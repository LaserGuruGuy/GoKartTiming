//-----------------------------------------------------------------------------
//
// <copyright file="ObservableDictionary.cs" company="Microsoft">
//    Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// Description:
//     ContentLocatorPart represents a set of name/value pairs that identify a
//     piece of data within a certain context.  The names and values are
//     strings.
//
//     Spec: http://team/sites/ag/Specifications/Simplifying%20Store%20Cache%20Model.doc
//
// History:
//  05/06/2004: ssimova:  Created
//  06/30/2004: rruiz:    Added change notifications to parent, clean-up
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;


namespace GoKartTiming.LiveTiming
{
    /// <summary>
    ///     ContentLocatorPart represents a set of name/value pairs that identify a
    ///     piece of data within a certain context.  The names and values are
    ///     all strings.
    /// </summary>
    public class ObservableDictionary : IDictionary<int?, int?>, INotifyPropertyChanged
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     Creates a ContentLocatorPart with the specified type name and namespace.
        /// </summary>
        public ObservableDictionary()
        {
            _nameValues = new Dictionary<int?, int?>();
        }

        #endregion Constructors

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        /// <summary>
        ///     Adds a key/value pair to the ContentLocatorPart.  If a value for the key already
        ///     exists, the old value is overwritten by the new value.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="val">value</param>
        /// <exception cref="ArgumentNullException">key or val is null</exception>
        /// <exception cref="ArgumentException">a value for key is already present in the locator part</exception>
        public void Add(int? key, int? val)
        {
            if (key == null || val == null)
            {
                throw new ArgumentNullException(key == null ? "key" : "val");
            }
            _nameValues.Add(key, val);
            FireDictionaryChanged();
        }

        /// <summary>
        ///     Removes all name/value pairs from the ContentLocatorPart.
        /// </summary>
        public void Clear()
        {
            int count = _nameValues.Count;

            if (count > 0)
            {
                _nameValues.Clear();

                // Only fire changed event if the dictionary actually changed
                FireDictionaryChanged();
            }
        }

        /// <summary>
        ///     Returns whether or not a value of the key exists in this ContentLocatorPart.
        /// </summary>
        /// <param name="key">the key to check for</param>
        /// <returns>true - yes, false - no</returns>
        public bool ContainsKey(int? key)
        {
            return _nameValues.ContainsKey(key);
        }

        /// <summary>
        ///     Removes the key and its value from the ContentLocatorPart.
        /// </summary>
        /// <param name="key">key to be removed</param>
        /// <returns>true - the key was found in the ContentLocatorPart, false o- it wasn't</returns>
        public bool Remove(int? key)
        {
            bool exists = _nameValues.Remove(key);

            // Only fire changed event if the key was actually removed
            if (exists)
            {
                FireDictionaryChanged();
            }

            return exists;
        }

        /// <summary>
        ///     Returns an enumerator for the key/value pairs in this ContentLocatorPart.
        /// </summary>
        /// <returns>an enumerator for the key/value pairs; never returns null</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nameValues.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator forthe key/value pairs in this ContentLocatorPart.
        /// </summary>
        /// <returns>an enumerator for the key/value pairs; never returns null</returns>
        public IEnumerator<KeyValuePair<int?, int?>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int?, int?>>)_nameValues).GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public bool TryGetValue(int? key, out int? value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return _nameValues.TryGetValue(key, out value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pair"></param>
        /// <exception cref="ArgumentNullException">pair is null</exception>
        void ICollection<KeyValuePair<int?, int?>>.Add(KeyValuePair<int?, int?> pair)
        {
            ((ICollection<KeyValuePair<int?, int?>>)_nameValues).Add(pair);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">pair is null</exception>
        bool ICollection<KeyValuePair<int?, int?>>.Contains(KeyValuePair<int?, int?> pair)
        {
            return ((ICollection<KeyValuePair<int?, int?>>)_nameValues).Contains(pair);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">pair is null</exception>
        bool ICollection<KeyValuePair<int?, int?>>.Remove(KeyValuePair<int?, int?> pair)
        {
            return ((ICollection<KeyValuePair<int?, int?>>)_nameValues).Remove(pair);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        /// <exception cref="ArgumentNullException">target is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than the lenght of target</exception>
        void ICollection<KeyValuePair<int?, int?>>.CopyTo(KeyValuePair<int?, int?>[] target, int startIndex)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (startIndex < 0 || startIndex > target.Length)
                throw new ArgumentOutOfRangeException("startIndex");

            ((ICollection<KeyValuePair<int?, int?>>)_nameValues).CopyTo(target, startIndex);
        }

        #endregion Public Methods

        //------------------------------------------------------
        //
        //  Public Operators
        //
        //------------------------------------------------------
        //------------------------------------------------------
        //
        //  Public Events
        //
        //------------------------------------------------------
        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        #region Public Properties

        /// <summary>
        ///     The number of name/value pairs in this ContentLocatorPart.
        /// </summary>
        /// <value>count of name/value pairs</value>
        public int Count
        {
            get
            {
                return _nameValues.Count;
            }
        }

        /// <summary>
        ///     Indexer provides lookup of values by key.  Gets or sets the value
        ///     in the ContentLocatorPart for the specified key.  If the key does not exist
        ///     in the ContentLocatorPart,
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>the value stored in this locator part for key</returns>
        public int? this[int? key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                int? value = null;
                _nameValues.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                int? oldValue = null;
                _nameValues.TryGetValue(key, out oldValue);

                // If the new value is actually different, then we add it and fire
                // a change notification
                if ((oldValue == null) || (oldValue != value))
                {
                    _nameValues[key] = value;
                    FireDictionaryChanged();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns a collection of all the keys in this ContentLocatorPart.
        /// </summary>
        /// <value>keys</value>
        public ICollection<int?> Keys
        {
            get
            {
                return _nameValues.Keys;
            }
        }

        /// <summary>
        ///     Returns a collection of all the values in this ContentLocatorPart.
        /// </summary>
        /// <value>values</value>
        public ICollection<int?> Values
        {
            get
            {
                return _nameValues.Values;
            }
        }

        #endregion Public Properties

        //------------------------------------------------------
        //
        //  Public Events
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------
        //------------------------------------------------------
        //
        //  Internal Operators
        //
        //------------------------------------------------------
        //------------------------------------------------------
        //
        //  Internal Events
        //
        //------------------------------------------------------

        #region Public Events

        /// <summary>
        ///
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------
        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private Methods

        /// <summary>
        ///     Notify the owner this ContentLocatorPart has changed.
        /// </summary>
        private void FireDictionaryChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(null));
            }
        }

        #endregion Private Methods

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private Fields

        /// <summary>
        ///     The internal data structure.
        /// </summary>
        private Dictionary<int?, int?> _nameValues;

        #endregion Private Fields
    }
}