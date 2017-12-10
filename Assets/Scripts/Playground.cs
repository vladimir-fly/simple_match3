using System;
using System.Collections;
using System.Collections.Generic;

namespace SM3
{
    public class Playground : IList<byte>
    {
        public byte Width { get; private set; }
        public byte Height { get; private set; }
        public byte ElementTypesCount { get; private set; }
        private List<byte> _elements;

        public Action OnSwaped;
        public Action OnRearrange;
        public Action OnFilled;

        public Playground(byte n, byte m, byte elementTypesCount)
        {
            Width = n;
            Height = m;
            ElementTypesCount = elementTypesCount;

            _elements = new List<byte>();
            for (var i = 0; i < n * m; i++)
                Add((byte) new Random().Next(1, elementTypesCount));
        }

        public byte this[int index]
        {
            get { return _elements[index]; }
            set { _elements[index] = value; }
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public void Add(byte item)
        {
            _elements.Add(item);
        }

        public bool Remove(byte item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            _elements[index] = 0;
        }

        public int IndexOf(byte item)
        {
            return _elements.IndexOf(item);
        }

        public void Insert(int index, byte item)
        {
            _elements[index] = item;
        }

        public bool CanSwap(int source, int target)
        {
            return CheckSwap(target, _elements[source]) || CheckSwap(source, _elements[target]);
        }

        private bool CheckSwap(int index, byte type)
        {
            return CheckRow(index, type) || CheckColumn(index, type);
        }

        private bool CheckRow(int index, byte type)
        {
            return 
                (_elements[++index] == type && _elements[--index] == type) ||
                (_elements[++index] == type && _elements[index + 2] == type) || 
                (_elements[--index] == type && _elements[index - 2] == type);
        }

        private bool CheckColumn(int index, byte type)
        {
            return 
                (_elements[++index] == type && _elements[--index] == type) || 
                (_elements[++index] == type && _elements[--index] == type) || 
                (_elements[++index] == type && _elements[--index] == type);
        }

        public void Swap(int source, int target)
        {
            var elementType = _elements[target];
            _elements[target] = _elements[source];
            _elements[source] = elementType;

            OnSwaped?.Invoke();
        }

        public void Clean()
        {
            GetRemovableElements().ForEach(RemoveAt);
        }

        private List<int> GetRemovableElements()
        {
            var result = new List<int>();
            return result;
        }

        public void Rearrange()
        {
            OnRearrange?.Invoke();
        }

        public void Fill()
        {
            OnFilled?.Invoke();
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(byte item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public override string ToString()
        {
            return $"N:{Width}, M:{Height}, ElementTypesCount:{ElementTypesCount}";
        }
    }
}