using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
  public class Node : IComparable<Node>
  {
    public static int generatedNode = 0;
    public int[] state { get; set; }
    public Node parent { get; set; }
    public List<Node> children { get; set; }
    public int cost { get; set; }
    public int depth { get; set; }

    public Node(int[] state)
    {
      this.state = state;
      children = new List<Node>();
      generatedNode++;
    }

    public Node Clone()
    {
      return new Node(state)
      {
        parent = parent,
        children = children,
        cost = cost,
        depth = depth
      };
    }

    public void AddChild(Node child)
    {
      children.Add(child);
    }

    public void SetParent(Node parent)
    {
      this.parent = parent;
    }

    public static void Reset()
    {
      generatedNode = 0;
    }

    public int CompareTo(Node other)
    {
      int compare1 = other.cost.CompareTo(this.cost);
      if (compare1 != 0)
      {
        return compare1;
      }

      return other.depth.CompareTo(this.depth);
    }
  }
}
