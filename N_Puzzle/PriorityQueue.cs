using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private LinkedList<T> _items;

        public PriorityQueue()
        {
            _items = new LinkedList<T>();
        }

        public void Enqueue(T Data)
        {
            if (Count == 0)
            {
                _items.AddLast(Data);
                return;
            }

            var current = _items.First;

            while (current != null && current.Value.CompareTo(Data) >= 0)
            {
                current = current.Next;
            }

            if (current == null)
            {
                _items.AddLast(Data);
            }
            else
            {
                _items.AddBefore(current, Data);
            }
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            var node = _items.First.Value;

            _items.RemoveFirst();

            return node;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            return _items.First.Value;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }
    }
}
