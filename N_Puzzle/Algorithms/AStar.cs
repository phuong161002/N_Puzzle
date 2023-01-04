using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
  public class AStar : ISolver
  {
    public Node startNode;

    public Node goalNode;

    public int CostH;

    public int CostG;

    HashSet<long> closed;
    public AStar(Node start, Node goal)
    {
      this.startNode = start;
      this.goalNode = goal;
    }
    public Node Solve(Node start, Node goal)
    {
      var listNode = new PriorityQueue<Node>();
      Node currentNode = new Node(start.state);
      closed = new HashSet<long>();
      listNode.Enqueue(currentNode);
      Util.Print(currentNode.state, "NODE BAN DAU");


      while (!MainForm.IsOutOfMem)
      {
        if (listNode.IsEmpty)
        {
          return null;
        }

        currentNode = listNode.Dequeue();

        Util.Print(currentNode.state, "NODE DUOC DUNG");

        if (Util.IsGoalState(currentNode, goal))
        {
          return currentNode;
        }
        else if (!Check(currentNode.state))
        {
          closed.Add(encode(currentNode.state));
          for (int i = 0; i < 4; i++)
          {
            if (Util.TryMove(currentNode, (MoveDirection)i, out Node nextNode) && !Check(nextNode.state))
            {
              CalculateCost(nextNode);
              Util.Print(nextNode.state, $"NODE MOI DUOC THEM VAO ____ {(MoveDirection)i}");
              Console.WriteLine($"Heuristic Cost {nextNode.cost}");
              Console.WriteLine($"Depth {nextNode.depth}");
              listNode.Enqueue(nextNode);
            }
          }
        }
      }
      return currentNode;
    }



    public int GetMisplacedTiles(Node nextNode)
    {
      int count = 0;
      for (int i = 0; i < nextNode.state.Length; i++)
      {
        if (nextNode.state[i] != goalNode.state[i])
        {
          count++;
        }
      }
      return count;
    }

    public int GetManhattanDistanceCost(Node nextNode)
    {
      int result = 0;
      int[] goalIndex = new int[9];
      int[] stateIndex = new int[9];

      for (int i = 0; i < nextNode.state.Length; i++)
      {
        goalIndex[goalNode.state[i]] = i;
        stateIndex[nextNode.state[i]] = i;
      }

      for (int i = 0; i < stateIndex.Length; i++)
      {
        int gx = goalIndex[i] % 3;
        int gy = goalIndex[i] / 3;
        int x = stateIndex[i] % 3;
        int y = stateIndex[i] / 3;

        result += System.Math.Abs(gx - x) + System.Math.Abs(gy - y);
      }

      return result;
    }

    private void CalculateCost(Node nextNode)
    {
      if (nextNode.parent == null)
      {
        nextNode.depth = 0;
      }
      else
      {
        CostG = nextNode.depth + 1;
      }
      CostH = GetManhattanDistanceCost(nextNode);

      nextNode.cost = CostH + CostG;
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

    private bool Check(int[] state)
    {
      return closed.Contains(encode(state));
    }
  }
}
