using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class Demo : ISolver
    {
        public Demo()
        {

        }

        public Node Solve(Node start, Node goal)
        {
            Queue<Node> queue = new Queue<Node>();

            Node currentNode = start;

            while(!Util.IsGoalState(currentNode, goal) && !MainForm.IsOutOfMem)
            {
                for(int i = 0; i < 4; i++)
                {
                    if(Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode))
                    {
                        queue.Enqueue(nextNode);
                    }
                }

                currentNode = queue.Dequeue();
            }

            Console.WriteLine($"depth: {currentNode.depth}   generatedNode: {Node.generatedNode}");
            return currentNode;
        }
    }
}
