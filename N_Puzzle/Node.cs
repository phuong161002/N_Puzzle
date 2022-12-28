using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    public class Node: IComparable<Node>
    {
        public static int[] goalState;
        public static int generatedNode = 0;
        public int[] state { get; set; }
        public Node parent { get; set; }
        public List<Node> children { get; set; }
        public int cost { get; set; }
        public int depth { get; set; }

        public int CostH { get => depth; }
        public int CostG { get => Settings.TYPE_HEUR == 0 ? manhattanDistance(goalState) : numWrongTiles(goalState); }
        public int CostF { get => CostH + CostG; }

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

        //~Node()
        //{
        //    generatedNode--;
        //}

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

        public int numWrongTiles(int[] goalState)
        {
            int count = 0;
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] != goalState[i])
                {
                    count++;
                }
            }
            return count;
        }

        public int manhattanDistance(int[] goalState)
        {
            int result = 0;
            int[] goalIndex = new int[goalState.Length];
            int[] stateIndex = new int[goalState.Length];

            for (int i = 0; i < state.Length; i++)
            {
                goalIndex[goalState[i]] = i;
                stateIndex[state[i]] = i;
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

        public int CompareTo(Node other)
        {
            int compare1 = other.CostF.CompareTo(this.CostF);
            if(compare1 != 0)
            {
                return compare1;
            }

            return other.CostH.CompareTo(this.CostH);
        }
    }
}
