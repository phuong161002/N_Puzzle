using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle.Algorithms
{
    public class Demo : ISolver
    {
        public Demo()
        {

        }

        public Node Solve(Node start, Node goal)
        {
            if(Util.TryMove(start, MoveDirection.Up, out Node nextNode))
            {
                return nextNode;
            }

            return start;
        }
    }
}
