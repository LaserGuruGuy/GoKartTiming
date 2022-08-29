using System;
using System.Collections.ObjectModel;

namespace CpbTiming.SmsTiming
{
    public class KeyedTimeSpan
    {
        public int Key { get; set; }
        public TimeSpan Value { get; set; }
    }

    public class UniqueKeyedCollectionChangedEventArgs : EventArgs
    {
        private KeyedTimeSpan _changedItem;
        private ChangeType _changeType;
        private KeyedTimeSpan _replacedWith;

        public KeyedTimeSpan ChangedItem { get { return _changedItem; } }
        public ChangeType ChangeType { get { return _changeType; } }
        public KeyedTimeSpan ReplacedWith { get { return _replacedWith; } }

        public UniqueKeyedCollectionChangedEventArgs(ChangeType change,
            KeyedTimeSpan item, KeyedTimeSpan replacement)
        {
            _changeType = change;
            _changedItem = item;
            _replacedWith = replacement;
        }
    }

    public enum ChangeType
    {
        Added,
        Removed,
        Replaced,
        Cleared
    };

    public class UniqueKeyedCollection : KeyedCollection<int, KeyedTimeSpan>
    {
        public event EventHandler<UniqueKeyedCollectionChangedEventArgs> Changed;

        public UniqueKeyedCollection() : base(null, 0) { }

        protected override int GetKeyForItem(KeyedTimeSpan item)
        {
            return item.Key;
        }

        protected override void InsertItem(int index, KeyedTimeSpan newItem)
        {
            if (base.Contains(newItem.Key))
            {
                return;
            }

            base.InsertItem(index, newItem);

            EventHandler<UniqueKeyedCollectionChangedEventArgs> temp = Changed;
            if (temp != null)
            {
                temp(this, new UniqueKeyedCollectionChangedEventArgs(
                    ChangeType.Added, newItem, null));
            }
        }

        protected override void SetItem(int index, KeyedTimeSpan newItem)
        {
            KeyedTimeSpan replaced = Items[index];
            base.SetItem(index, newItem);

            EventHandler<UniqueKeyedCollectionChangedEventArgs> temp = Changed;
            if (temp != null)
            {
                temp(this, new UniqueKeyedCollectionChangedEventArgs(
                    ChangeType.Replaced, replaced, newItem));
            }
        }

        protected override void RemoveItem(int index)
        {
            KeyedTimeSpan removedItem = Items[index];
            base.RemoveItem(index);

            EventHandler<UniqueKeyedCollectionChangedEventArgs> temp = Changed;
            if (temp != null)
            {
                temp(this, new UniqueKeyedCollectionChangedEventArgs(
                    ChangeType.Removed, removedItem, null));
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            EventHandler<UniqueKeyedCollectionChangedEventArgs> temp = Changed;
            if (temp != null)
            {
                temp(this, new UniqueKeyedCollectionChangedEventArgs(
                    ChangeType.Cleared, null, null));
            }
        }

        public TimeSpan GetValue(int Key)
        {
            foreach (var Item in Items)
            {
                if (Item.Key.Equals(Key))
                {
                    return Item.Value;
                }
            }
            return TimeSpan.Zero;
        }
    }
}
