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

      while (!MainForm.IsOutOfMem)
      {
        if (listNode.IsEmpty)
        {
          return null;
        }

        currentNode = listNode.Dequeue();

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
              listNode.Enqueue(nextNode);
            }
          }
        }
      }
      return currentNode;
    }



    public int numWrongTiles(Node nextNode)
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

    public int manhattanDistance(Node nextNode)
    {
      int result = 0;
      int[] goalIndex = new int[goalNode.state.Length];
      int[] stateIndex = new int[goalNode.state.Length];

      for (int i = 0; i < nextNode.state.Length; i++)
      {
        goalIndex[goalNode.state[i]] = i;
        stateIndex[nextNode.state[i]] = i;
      }

      for (int i = 0; i < stateIndex.Length; i++)
      {
        int goalX = goalIndex[i] % Settings.SIZE;
        int goalY = goalIndex[i] / Settings.SIZE;
        int stateX = stateIndex[i] % Settings.SIZE;
        int stateY = stateIndex[i] / Settings.SIZE;

        result += Math.Abs(goalX - stateX) + Math.Abs(goalY - stateY);
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
      CostH = manhattanDistance(nextNode);

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
