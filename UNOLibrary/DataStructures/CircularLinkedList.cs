/* File Name:       CircularLinkedList.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Friday, April April 8, 2022
 * Brief:           A generic class which represents a circular linked list data structure, where the head and
 *                  tail nodes are always connected to each other. */

using System.Collections;
using System.Collections.Generic;

namespace UNOLibrary.DataStructures
{
    public sealed class CircularLinkedList<T> : IEnumerable
    {
        private CircularLinkedListNode<T> _currentNode = null;

        private List<CircularLinkedListNode<T>> _nodes = null;

        public int Length => _nodes is null ? 0 : _nodes.Count;

        public T CurrentNodeData => _currentNode.Data;

        public CircularLinkedList()
        {

        }

        public T this[int index]
        {
            get
            {
                return _nodes[index].Data;
            }
            set
            {
                _nodes[index].Data = value;
            }
        }

        public void AddToEnd(T node)
        {
            CircularLinkedListNode<T> nodeToAppend = new CircularLinkedListNode<T>(node);

            if (_nodes is null)
            {
                _nodes = new List<CircularLinkedListNode<T>>();

                nodeToAppend.Previous = nodeToAppend;
                nodeToAppend.Next = nodeToAppend;

                _nodes.Add(nodeToAppend);

                return;
            }
            
            // Update the pointers in the previous node at the end to point to the new item being inserted
            _nodes[Length - 1].Next = nodeToAppend;
            _nodes[0].Previous = nodeToAppend;

            nodeToAppend.Previous = _nodes[Length - 1];
            nodeToAppend.Next = _nodes[0];

            _nodes.Add(nodeToAppend);
        }

        public int MovePrevious()
        {
            if (_nodes is null)
            {
                return -1;
            }

            if (Length > 0)
            {
                if (_currentNode is null)
                {
                    _currentNode = _nodes[0];
                }

                _currentNode = _currentNode.Previous;

                return _nodes.IndexOf(_currentNode);
            }

            return -1;
        }

        public int MoveNext()
        {
            if (_nodes is null)
            {
                return -1;
            }

            if (Length > 0)
            {
                if (_currentNode is null)
                {
                    _currentNode = _nodes[0];
                }

                _currentNode = _currentNode.Next;

                return _nodes.IndexOf(_currentNode);
            }

            return -1;
        }

        public int PeekMoveNext()
        {
            if (_nodes is null)
            {
                return -1;
            }
            
            return _nodes.IndexOf(_currentNode.Next);
        }

        public int PeekMovePrevious()
        {
            if (_nodes is null)
            {
                return -1;
            }

            return _nodes.IndexOf(_currentNode.Previous);
        }

        public bool RemoveFirst(T nodeToRemove)
        {
            if (Length == 0)
            {
                return false;
            }

            int nodeIndex = _nodes.FindIndex(node => node.Data.Equals(nodeToRemove));
            CircularLinkedListNode<T> nodeAtIndex = _nodes[nodeIndex];

            if (nodeIndex != -1)
            {
                if (nodeIndex == 0) // Node is first item
                {
                    if (Length - 1 > 0)
                    {
                        _nodes[nodeIndex + 1].Previous = _nodes[Length - 1]; // Update element next to first element (on right)
                        _nodes[Length - 1].Next = _nodes[nodeIndex + 1]; // Update element element next to first element (on left/tail)
                    }
                }
                else if (nodeIndex == Length - 1) // Node is last item
                {
                    if (Length - 1 > 0)
                    {
                        _nodes[0].Previous = _nodes[Length - 2];
                        _nodes[Length - 2].Next = _nodes[0];
                    }
                }
                else // Node is between head and tail
                {
                    _nodes[nodeIndex - 1].Next = nodeAtIndex.Next;
                    _nodes[nodeIndex + 1].Previous = nodeAtIndex.Previous;
                }

                _nodes.RemoveAt(nodeIndex);

                return true;
            }

            return false;
        }

        public List<T> ToList()
        {
            List<T> items = new List<T>();

            for (int i = 0; i < _nodes.Count; i++)
            {
                items.Add(_nodes[i].Data);
            }

            return items;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (CircularLinkedListNode<T> item in _nodes)
            {
                yield return item.Data;
            }
        }
    }
}
