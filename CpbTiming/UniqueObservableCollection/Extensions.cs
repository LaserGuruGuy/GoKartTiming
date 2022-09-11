using System.Collections.Generic;
using System.Linq;

namespace CpbTiming.SmsTiming
{
    public static class Extensions
    {
        public static void Sort<T>(this UniqueObservableCollection<T> Me)
        {
            if (Me?.GetType() == typeof(UniqueObservableCollection<DriverEx>))
            {
                UniqueObservableCollection<DriverEx> collection = Me as UniqueObservableCollection<DriverEx>;

                for (var i = Me.Count() - 1; i > 0; i--)
                {
                    for (var j = 1; j <= i; j++)
                    {
                        DriverEx o1 = collection[j - 1];
                        DriverEx o2 = collection[j];

                        if (o1.Position > o2.Position)
                        {
                            collection.Remove(o1);
                            collection.Insert(j, o1);
                        }
                    }
                }
            }
        }

        public static TKey SmartReverseLookup<TKey, TValue>(this Dictionary<TKey, TValue> Me, TValue Value, TKey DefaultIfEmpty)
        {
            try
            {
                if (Me.ContainsValue(Value))
                    return Me.First(a => a.Value.Equals(Value)).Key;

                return DefaultIfEmpty;
            }
            catch
            {
                return DefaultIfEmpty;
            }
        }

        public static TKey[] ReverseLookup<TKey, TValue>(this Dictionary<TKey, TValue> Me, TValue Value)
        {
            try
            {
                if (Me.ContainsValue(Value))
                    return Me.Where(a => a.Value.Equals(Value)).Select(b => b.Key).ToArray();
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}