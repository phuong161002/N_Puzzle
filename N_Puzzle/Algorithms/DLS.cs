﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class DLS : ISolver
    {
        public Node GoalNode { get; private set; }

        public SolvingStatus Status { get; private set; }

        public event Action OnSolvingCompleted;
        public event Action OnSolvingFailed;

        HashSet<string> closed;

        public DLS()
        {
            closed = new HashSet<string>();
        }

        public void Solve(int[] start, int[] goal)
        {
            Node startNode = new Node(start);
            Stack<Node> openNode = new Stack<Node>();
            openNode.Push(startNode);
            closed.Clear();

            Node currentNode;
            while (openNode.Count > 0)
            {
                currentNode = openNode.Pop();

                if (Utils.IsGoalState(currentNode.state, goal))
                {
                    GoalNode = currentNode;
                    OnSolvingCompleted?.Invoke();
                    return;
                }
                Node.NumEvaluatedNodes++;

                closed.Add(Utils.EncodeNode(currentNode.state, currentNode.depth));
                if (currentNode.depth < Settings.MaxDepthDLS)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Utils.TryMove(currentNode, (MoveDirection)i, out Node nextNode)
                            && !closed.Contains(Utils.EncodeNode(nextNode.state, nextNode.depth)))
                        {
                            openNode.Push(nextNode);
                        }
                    }
                }
            }

            GoalNode = null;
            OnSolvingFailed?.Invoke();
        }
    }
}
