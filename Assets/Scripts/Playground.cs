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
            return CheckRowNeighbors(index, type) || CheckColumnNeighbors(index, type);
        }

        private bool CheckRowNeighbors(int index, byte type)
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

        private bool CheckColumnNeighbors(int index, byte type)
        {
            var topBorder = index % Width;
            var bottomBorder = topBorder + (Height - 1) * Width;
            
            var topIndex1 = index - Width;
            var topIndex2 = index - 2 * Width;

            var bottomIndex1 = index + Width;
            var bottomIndex2 = index + 2 * Width;

            return 
                (topIndex1 >= topBorder && bottomIndex1 <= bottomBorder && _elements[topIndex1] == type && _elements[bottomIndex1] == type) ||
                (topIndex1 >= topBorder && topIndex2 >= topBorder && _elements[topIndex1] == type && _elements[topIndex2] == type) || 
                (bottomIndex1 <= bottomBorder && bottomIndex2 <= bottomBorder && _elements[bottomIndex1] == type && _elements[bottomIndex2] == type);
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
            Debug.Log(PrettyLog.GetMessage(nameof(Playground), nameof(CleanAt), $"{index}"));
            var list = GetRemovableElements(index);

            if (list.Count == 1) return;

            list.ForEach(RemoveAt);
            
            var result = list.Select(element => Tuple.Create(element, (byte) 0)).ToList();
            Debug.Log(PrettyLog.GetMessage(nameof(Playground), nameof(CleanAt), result));
            OnPlaygroundUpdated?.Invoke(result);
        }

        private List<int> GetRemovableElements(int index)
        {
            var result = new List<int>() { index };

            while(true)
            {
                var elements = GetAllAvailableElements(result);
                if (!elements.Any()) break;
                result.AddRange(elements);
            }

            Debug.Log(PrettyLog.GetMessage(nameof(Playground), nameof(GetRemovableElements), result));
            return result.Distinct().ToList();
        }

        private List<int> GetAllAvailableElements(List<int> indexes)
        {
            var result = new List<int>();
            
            indexes.ForEach(index => result.AddRange(GetMaxHorizontallSubset(index)));
            indexes.ForEach(index => result.AddRange(GetMaxVerticalSubset(index)));

            return result.Distinct().Except(indexes).ToList();
        }

        private List<int> GetMaxHorizontallSubset(int index)
        {
            var result = new List<int>();
            if (!CheckRowNeighbors(index, _elements[index]))
                return result;
            
            var rowIndex = index / Width;
            var leftBorder = rowIndex * Width;
            var rightBorder = (rowIndex + 1) * Width - 1;
            var elementType = _elements[index];

            for (var leftIndex = index; leftIndex >= leftBorder; leftIndex--)
            {
                if (leftIndex >= leftBorder && _elements[leftIndex] == elementType) 
                    result.Add(leftIndex);
                else break;
            }

            for (var rightIndex = index; rightIndex <= rightBorder; rightIndex++)
            {
                if (rightIndex <= rightBorder && _elements[rightIndex] == elementType)
                    result.Add(rightIndex);
                else break;
            }

            return result;
        }

        private List<int> GetMaxVerticalSubset(int index)
        {
            var result = new List<int>();
            if (!CheckColumnNeighbors(index, _elements[index]))
                return result;

            var topBorder = index % Width;
            var bottomBorder = topBorder + (Height - 1) * Width;
            var elementType = _elements[index];

            for (var topIndex = index; topIndex >= topBorder; topIndex -= Width)
            {
                if (topIndex >= topBorder && _elements[topIndex] == elementType) 
                    result.Add(topIndex);
                else break;
            }

            for (var bottomIndex = index; bottomIndex <= bottomBorder; bottomIndex += Width)
            {
                if (bottomIndex <= bottomBorder && _elements[bottomIndex] == elementType)
                    result.Add(bottomIndex);
                else break;
            }

            return result;;
        }

        public void Rearrange()
        {
            var rearrangedElements = new List<Tuple<int, byte>>();

            for (var i = 0; i < Width; i++)
                rearrangedElements.AddRange(GetSortedColumn(i));

            OnPlaygroundUpdated?.Invoke(rearrangedElements);

            Debug.Log(PrettyLog.GetMessage(nameof(Playground), nameof(Rearrange), rearrangedElements));
        }

        private List<Tuple<int, byte>> GetSortedColumn(int columnIndex)        
        {
            var result = new List<Tuple<int, byte>>();

            var bottomBorder = columnIndex + (Height - 1) * Width;
            for (var i = bottomBorder; i >= columnIndex; i -= Width)
            {
                if (_elements[i] == 0)
                {
                    for (var j = i; j >= columnIndex; j -= Width)
                    {
                        if (_elements[j] != 0)
                        {
                            var elementType = _elements[i];
                            _elements[i] = _elements[j];
                            _elements[j] = elementType;
                        }
                    }
                }
            }

            for (var i = bottomBorder; i >= columnIndex; i -= Width)
            {
                result.Add(Tuple.Create(i, _elements[i]));
            }

            return result;
        }

        public void Fill()
        {
            var filledElements = new List<Tuple<int, byte>>();
            var i = 0;
            
            _elements.ForEach(element =>
            {
                if (element == 0)
                    filledElements.Add(Tuple.Create(i, 
                        (byte) new Random().Next(1, new Random().Next(1, ElementTypesCount))));
                i++;
            });
            
            filledElements.ForEach(element =>_elements[element.Item1] = element.Item2);

            OnPlaygroundUpdated?.Invoke(filledElements);
            Debug.Log(PrettyLog.GetMessage(nameof(Playground), nameof(Fill), filledElements));
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