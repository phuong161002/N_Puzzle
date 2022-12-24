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

      throw new NotImplementedException();
    }



    private int GetManhattanDistanceCost()
    {
      int heuristicCost = 0;
      for (int i = 0; i < currentNode.state.Length; i++)
      {
        int v = currentNode.state[i] - 1;
        if (v == -1)
        {
          v = currentNode.state.Length - 1;
          continue;
        }
        if (v == currentNode.state.Length - 1) continue;

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

    private int GetMisplacedTiles()
    {
      int heuristicCost = 0;
      for (int i = 0; i < currentNode.state.Length; i++)
      {
        int v = currentNode.state[i] - 1;
        if (v == -1)
        {
          v = currentNode.state.Length - 1;
          continue;
        }
        if (v != i) heuristicCost++;
      }
      return heuristicCost;
    }

    private int GetHeuristicCost()
    {
      return GetManhattanDistanceCost();
    }

    private void CalculateCost()
    {
      if (currentNode.parent == null)
      {
        Costg = 0;
      }
      else
      {
        Costg = currentNode.depth + 1;
      }
      Costh = GetHeuristicCost();

      currentNode.cost = Costh + Costg;
    }
  }







}
