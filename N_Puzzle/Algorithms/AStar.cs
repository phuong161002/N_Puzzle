using System.ComponentModel;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class AStar : ISolver
    {
        public Node GoalNode { get; private set; }
        public SolvingStatus Status { get; private set; }


        private HashSet<string> closed;

        public AStar()
        {
            closed = new HashSet<string>();
        }

        public event Action OnSolvingCompleted;
        public event Action OnSolvingFailed;

        public void Solve(int[] start, int[] goal)
        {
            Node startNode = new Node(start);
            PriorityQueue<Node> leaves = new PriorityQueue<Node>();
            leaves.Enqueue(startNode);
            Node currentNode;
            closed.Clear();
            while (!leaves.IsEmpty)
            {
                currentNode = leaves.Dequeue();

                if (Utils.IsGoalState(currentNode.state, goal))
                {
                    GoalNode = currentNode;
                    OnSolvingCompleted?.Invoke();
                    return;
                }

                Node.NumEvaluatedNodes++;
                closed.Add(Utils.EncodeNode(currentNode.state));
                for (int i = 0; i < 4; i++)
                {
                    if (Utils.TryMove(currentNode, (MoveDirection)i, out Node nextNode) && !CheckIfStateExisted(nextNode.state))
                    {
                        leaves.Enqueue(nextNode);
                    }
                }
            }

            GoalNode = null;
            OnSolvingFailed?.Invoke();
        }


        private bool CheckIfStateExisted(int[] state)
        {
            return closed.Contains(Utils.EncodeNode(state));
        }
    }
}
