using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class BFS : ISolver
    {
        public Node GoalNode { get; private set; }

        public SolvingStatus Status { get; private set; }

        public event Action OnSolvingCompleted;
        public event Action OnSolvingFailed;

        private HashSet<string> closed;
        private Queue<Node> openNodes;

        public BFS()
        {
            closed = new HashSet<string>();
            openNodes = new Queue<Node>();
        }

        public void Solve(int[] start, int[] goal)
        {
            Status = SolvingStatus.Solving;
            Node startNode = new Node(start);

            openNodes.Clear();
            openNodes.Enqueue(startNode);

            Node currentNode;
            while (openNodes.Count > 0)
            {
                currentNode = openNodes.Dequeue();

                if (Utils.IsGoalState(currentNode.state, goal))
                {
                    GoalNode = currentNode;
                    Status = SolvingStatus.Success;
                    OnSolvingCompleted?.Invoke();
                    return;
                }

                Node.NumEvaluatedNodes++;
                closed.Add(Utils.EncodeNode(currentNode.state));
                for (int i = 0; i < 4; i++)
                {
                    if (Utils.TryMove(currentNode, (MoveDirection)i, out Node nextNode)
                        && !closed.Contains(Utils.EncodeNode(nextNode.state)))
                    {
                        Node.NumNodesInTree++;
                        openNodes.Enqueue(nextNode);
                    }
                }
            }

            GoalNode = null;
            Status = SolvingStatus.Failed;
            OnSolvingFailed?.Invoke();
        }
    }
}
