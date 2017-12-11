using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;

using UnityEngine;

using SM3.Helpers;

namespace SM3
{
    public class Playground : IList<byte>
    {
        public byte Width { get; private set; }
        public byte Height { get; private set; }
        public byte ElementTypesCount { get; private set; }
        private List<byte> _elements;

        public Action<List<Tuple<int, byte>>> OnPlaygroundUpdated;

        public Playground(byte n, byte m, byte elementTypesCount)
        {
            Width = n;
            Height = m;
            ElementTypesCount = elementTypesCount;

            _elements = new List<byte>();
            for (var i = 0; i < n * m; i++)
                Add((byte) new Random().Next(1, new Random().Next(1, new Random().Next(1, elementTypesCount))));

            Debug.Log(PrettyLog.GetMessage(nameof(Playground), "Ctor", _elements));
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
            var lineIndex = index / Width;
            
            var leftBorder = lineIndex * Width + 1;
            var rightBorder = (index + 1) * Width + 1;
            
            var leftIndex1 = index - 1;
            var leftIndex2 = index - 2;

            var rightIndex1 = index + 1;
            var rightIndex2 = index + 2;

            return 
                (leftIndex1 >= leftBorder && rightIndex1 <= rightBorder && _elements[leftIndex1] == type && _elements[rightIndex1] == type) ||
                (leftIndex1 >= leftBorder && leftIndex2 >= leftBorder && _elements[leftIndex1] == type && _elements[leftIndex2] == type) || 
                (rightIndex1 <= rightBorder && rightIndex2 <= rightBorder && _elements[rightIndex1] == type && _elements[rightIndex2] == type);
        }

        private bool CheckColumn(int index, byte type)
        {
            return false;
                // (_elements[++index] == type && _elements[--index] == type) || 
                // (_elements[++index] == type && _elements[--index] == type) || 
                // (_elements[++index] == type && _elements[--index] == type);
        }

        public void Swap(int source, int target)
        {
            var elementType = _elements[target];
            _elements[target] = _elements[source];
            _elements[source] = elementType;

            var updatedElementes = 
                new[] 
                {
                    Tuple.Create(source, _elements[source]), 
                    Tuple.Create(target, _elements[target])
                }.ToList();

            OnPlaygroundUpdated?.Invoke(updatedElementes);
        }

        public void CleanAt(int index)
        {
            var list = GetRemovableElements(index);
            list.ForEach(RemoveAt);

            var result = new List<Tuple<int, byte>>();

            foreach (var e in list)
                result.Add(Tuple.Create(e, (byte) 0));

            OnPlaygroundUpdated?.Invoke(result);

            Debug.Log(PrettyLog.GetMessage<int>(CleanAt, $"{index}"));
        }

        private List<int> GetRemovableElements(int index)
        {
            return new List<int>() { index - 1, index, index + 1 };
        }

        public void Rearrange()
        {
            //OnPlaygroundUpdated?.Invoke(new List<Tuple<int, byte>>());

            Debug.Log(PrettyLog.GetMessage(Rearrange));
        }

        public void Fill()
        {
            //OnPlaygroundUpdated?.Invoke(new List<Tuple<int, byte>>());

            Debug.Log(PrettyLog.GetMessage(Fill));
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