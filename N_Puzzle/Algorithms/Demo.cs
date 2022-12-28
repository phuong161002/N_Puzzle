using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class Demo : ISolver
    {
        private class AStarNode : Node
        {
            public AStarNode(int[] state) : base(state)
            {
            }

            public int numWrongTiles(int[] goalState)
            {
                int count = 0;
                for(int i = 0; i < state.Length; i++)
                {
                    if(state[i] != goalState[i])
                    {
                        count++;
                    }
                }
                return count;
            }

            public int manhattanDistance(int[] goalState)
            {
                for(int i = 0; i < state.Length; i++)
                {

                }
            }
        }
        public Demo()
        {

        }

        public Node Solve(Node start, Node goal)
        {
            var leaves = new PriorityQueue<Node>();
            var currentNode = new AStarNode(start.state);
            leaves.Enqueue(currentNode, 10);

            while(true)
            {
                if(leaves.IsEmpty)
                {
                    return null;
                }
            }

            return currentNode;
        }
    }
}