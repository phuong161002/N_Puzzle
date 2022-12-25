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

      while (!Util.IsGoalState(currentNode, goal) && !MainForm.IsOutOfMem)
      {
        Node temp = null;

        for (int i = 0; i < 4; i++)
        {
          if (Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode))
          {
            CalculateCost(nextNode);
            Console.WriteLine($"cost: {nextNode.cost}");
            Console.WriteLine($"depth: {nextNode.depth}");
            Console.WriteLine($"i: {i}");
            for (int j = 0; j < nextNode.state.Length; j++)
            {
              Console.WriteLine($"nextNode demo, {nextNode.state[j]}");
            }
            if (temp == null)
            {
              temp = nextNode;
              continue;
            }
            else
              temp = CompareNode(temp, nextNode);
          }
        }
        currentNode = temp;
      }
      for (int i = 0; i < goal.state.Length; i++)
      {
        Console.WriteLine($"goal, {goal.state[i]}");
      }

      for (int i = 0; i < currentNode.state.Length; i++)
      {
        Console.WriteLine($"current, {currentNode.state[i]}");
      }

      Console.WriteLine($"depth: {currentNode.depth}  generatedNode: {Node.generatedNode}");
      Node.Reset();
      return currentNode;
    }



    private int GetManhattanDistanceCost(ref Node nextNode)
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

    private int GetMisplacedTiles(ref Node nextNode)
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
        if (v != i) heuristicCost++;
      }
      return heuristicCost;
    }

    private int GetHeuristicCost(Node nextNode)
    {
      return GetManhattanDistanceCost(ref nextNode);
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
  }







}
