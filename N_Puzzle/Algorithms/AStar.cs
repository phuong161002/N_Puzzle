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

        private int mSpaceIndex;

        private Node startNode;

        private Node goalNode;

        private Node currentNode;

        public Node endNode;

        public Node parent;

        private int Costh;

        private int Costg;

        public AStar(Node start, Node goal)
        {
            startNode = start;
            goalNode = goal;
        }

        public Node Solve(Node start, Node goal)
        {
            Node currentNode = new Node(start.state);
            Console.WriteLine("LUC BAT DAU");
            printf(currentNode.state);
            var listNode = new List<Node>();
            listNode.Add(currentNode);

            while (!Util.IsGoalState(currentNode, goal) && !MainForm.IsOutOfMem)
            {
                var historyNode = new List<Node>();
                int j = listNode.Count - 1;
                Console.WriteLine($"so j luc dau: {j}");

                for (int i = 0; i < 4; i++)
                {
                    if (Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode))
                    {
                        CalculateCost(nextNode);
                        Console.WriteLine($"cost for nextNode: {nextNode.cost}");
                        historyNode.Add(nextNode);
                        Util.Print(nextNode.state, "NODE MOI");
                        Console.WriteLine($"history node count: {historyNode.Count}");
                    }
                }
                listNode.RemoveAt(listNode.Count - 1);


                for (int i = 0; i < historyNode.Count; i++)
                {
                    for (int k = i + 1; k < historyNode.Count; k++)
                    {
                        if (historyNode[i].cost <= historyNode[k].cost)
                        {
                            ;
                            Swap(historyNode, i, k);

                        }
                    }
                }

                for (int i = 0; i < historyNode.Count; i++)
                {
                    if (historyNode[i].cost == historyNode[historyNode.Count - 1].cost)
                    {
                        listNode.Add(historyNode[i]);
                        printf(historyNode[i].state);
                        Console.WriteLine("__");
                    }
                }
                Console.WriteLine("__NEXT__");
                printf(listNode[j].state);
                currentNode = listNode[listNode.Count - 1];

            }
            printf(currentNode.state);
            Console.WriteLine($"depth: {currentNode.depth}  generatedNode: {Node.generatedNode}");
            Node.Reset();
            return currentNode;

        }



        private int GetManhattanDistanceCost(Node nextNode)
        {
            int heuristicCost = 0;
            for (int i = 0; i < nextNode.state.Length; i++)
            {
                int v = nextNode.state[i] - 1;
                if (v == -1)
                {
                    v = nextNode.state.Length - 1;
                    continue;
                }
                if (v == nextNode.state.Length - 1) continue;

                if (v != i)
                {
                    int gx = v % 3;
                    int gy = v / 3;

                    int x = i % 3;
                    int y = i / 3;

                    int manCost = System.Math.Abs(x - gx) + System.Math.Abs(y - gy);
                    heuristicCost += manCost;
                }
            }
            return heuristicCost;
        }

        private int GetMisplacedTiles(Node nextNode)
        {
            int heuristicCost = 0;
            for (int i = 0; i < nextNode.state.Length; i++)
            {
                int v = nextNode.state[i] - 1;
                if (v == -1)
                {
                    v = nextNode.state.Length - 1;
                }
                if (v != i) heuristicCost++;
            }
            return heuristicCost;
        }

        private int GetHeuristicCost(Node nextNode)
        {
            return GetMisplacedTiles(nextNode);
        }

        private void CalculateCost(Node nextNode)
        {
            if (nextNode.parent == null)
            {
                Costg = 0;
            }
            else
            {
                Costg = nextNode.depth + 1;
            }
            Costh = GetHeuristicCost(nextNode);

            nextNode.cost = Costh + Costg;
        }

        private Node CompareNode(Node node1, Node node2)
        {
            if (node1.cost >= node2.cost)
            {
                return node2;
            }
            else
            {
                return node1;
            }
        }

        private void Swap<T>(IList<T> list, int i, int j)
        {
            T temp = list.ElementAt(i);
            list[i] = list[j];
            list[j] = temp;
        }

        private void printf(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($" {arr[i]} ");
            }
        }
    }







}
