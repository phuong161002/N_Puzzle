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

        private LinkedList<PQNode> DataList;

        public PriorityQueue()
        {
            DataList = new LinkedList<PQNode>();
        }

        public void Enqueue(T Data, int priority)
        {
            PQNode newNode = new PQNode()
            {
                Data = Data,
                Priority = priority
            };


            var current = DataList.First;
            if(current == null)
            {
                DataList.AddFirst(newNode);
                return;
            }
            while (current.Next != null)
            {
                if(current.Value.Priority < priority)
                {
                    break;
                }
                current = current.Next;
            }

            DataList.AddBefore(current, newNode);
        }

        public T Dequeue()
        {
            var node = DataList.First.Value;

            DataList.RemoveFirst();

            return node.Data;
        }

        public int Count
        {
            get { return DataList.Count; }
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }
    }
}
