using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    public class PriorityQueue<T>
    {
        private class PQNode
        {
            public int Priority;
            public T Data;
        }

        private LinkedList<PQNode> _items;

        public PriorityQueue()
        {
            _items = new LinkedList<PQNode>();
        }

        public void Enqueue(T Data, int priority)
        {
            PQNode newNode = new PQNode()
            {
                Data = Data,
                Priority = priority
            };
            if (Count == 0)
            {
                _items.AddLast(newNode);
                return;
            }

            var current = _items.First;
            
            while(current != null && current.Value.Priority >= priority)
            {
                current = current.Next;
            }

            if(current == null)
            {
                _items.AddLast(newNode);
            }
            else
            {
                _items.AddBefore(current, newNode);
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

            return node.Data;
        }

        public T Peek()
        {
            if(_items.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            return _items.First.Value.Data;
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
