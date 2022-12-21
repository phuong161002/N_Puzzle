using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class BFS : ISolver
    {
        private Node startNode;
        private Node goalNode;

        public bool FoundSolution;
        public Node endNode;

        public BFS(Node start, Node goal)
        {
            startNode = start;
            goalNode = goal;
        }

        public Node Solve(Node start, Node goal)
        {
            Queue<Node> queue = new Queue<Node>();
            Node currentNode = new Node(start.state);
            while (!Util.IsGoalState(currentNode, goal) && !MainForm.IsOutOfMem)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode))
                    {
                        queue.Enqueue(nextNode);
                    }
                }

                currentNode = queue.Dequeue();
            }

            Console.WriteLine($"depth: {currentNode.depth}  generatedNode: {Node.generatedNode}");
            Node.Reset();
            return currentNode;
        }
    }
}
