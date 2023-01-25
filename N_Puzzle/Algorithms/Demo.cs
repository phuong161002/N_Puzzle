using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class Demo : ISolver
    {
        HashSet<long> closed;

        public Demo()
        {

        }

        public Node Solve(Node start, Node goal)
        {
            Node.goalState = goal.state;
            var leaves = new PriorityQueue<Node>();
            Node currentNode = new Node(start.state);
            closed = new HashSet<long>();
            leaves.Enqueue(currentNode);
            while (!MainForm.IsOutOfMem)
            {
                if (leaves.IsEmpty)
                {
                    return null;
                }

                currentNode = leaves.Dequeue();
                //Util.Print(currentNode.state);
                if(Util.IsGoalState(currentNode, goal))
                {
                    return currentNode;
                }
                else if(!Check(currentNode.state))
                {
                    closed.Add(encode(currentNode.state));
                    for(int i = 0; i < 4; i++)
                    {
                        if(Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode) && !Check(nextNode.state))
                        {
                            leaves.Enqueue(nextNode);
                        }
                    }
                }
            }

            return currentNode;
        }

        private long encode(int[] state)
        {
            long res = 0;
            for (int i = 0; i < state.Length; i++)
            {
                res |= (long)state[i];
                res <<= 4;
            }
            return res;
        }

        private bool Check( int[] state)
        {
            return closed.Contains(encode(state));
        }
    }
}