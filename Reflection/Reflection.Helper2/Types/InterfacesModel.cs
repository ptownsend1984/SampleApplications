using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2.Types
{
    public class InterfacesModel : IComparable<int>, IComparable, ICollection<int>, ICollection, IDictionary<int, int>
    {

        public int Value;
        private Collection<int> Collection = new Collection<int>();

        #region IComparable<int> Members

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is int))
                return 0;

            return Value.CompareTo((int)obj);
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            ((ICollection)Collection).CopyTo(array, index);
        }

        public int Count
        {
            get { return ((ICollection)Collection).Count; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)Collection).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)Collection).SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        #endregion

        #region ICollection<int> Members

        public void Add(int item)
        {
            ((ICollection<int>)Collection).Add(item);
        }

        public void Clear()
        {
            ((ICollection<int>)Collection).Clear();
        }

        public bool Contains(int item)
        {
            return ((ICollection<int>)Collection).Contains(item);
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            ((ICollection<int>)Collection).CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<int>)Collection).IsReadOnly; }
        }

        public bool Remove(int item)
        {
            return ((ICollection<int>)Collection).Remove(item);
        }

        #endregion

        #region IEnumerable<int> Members

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return ((ICollection<int>)Collection).GetEnumerator();
        }

        #endregion

        #region IDictionary<int,int> Members

        public void Add(int key, int value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(int key)
        {
            throw new NotImplementedException();
        }

        public ICollection<int> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool TryGetValue(int key, out int value)
        {
            throw new NotImplementedException();
        }

        public ICollection<int> Values
        {
            get { throw new NotImplementedException(); }
        }

        public int this[int key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<int,int>> Members

        public void Add(KeyValuePair<int, int> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<int, int> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<int, int>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<int, int> item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<KeyValuePair<int,int>> Members

        IEnumerator<KeyValuePair<int, int>> IEnumerable<KeyValuePair<int, int>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}