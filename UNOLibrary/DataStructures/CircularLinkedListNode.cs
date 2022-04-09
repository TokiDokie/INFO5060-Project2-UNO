/* File Name:       CircularLinkedListNode.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Friday, April April 8, 2022
 * Brief:           Generic class which holds a node for a circular linked list. This class holds a previous node, the next node, and the
 *                  data attached to the node. */

namespace UNOLibrary.DataStructures
{
    public sealed class CircularLinkedListNode<T>
    {
        public T Data { get; set; }
        public CircularLinkedListNode<T> Previous { get; set; }
        public CircularLinkedListNode<T> Next { get; set; }

        public CircularLinkedListNode(T data)
        {
            Data = data;
        }
    }
}
