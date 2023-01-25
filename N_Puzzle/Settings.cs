using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    public static class Settings
    {
        public static int SIZE = 3;
        public static int TypeHeuristic = 0;
        public static int[] DefaultGoalState = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        public static int[] GoalState = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        public static int[] StartState = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
        public static int DelayMove = 20; // ms
        public static int MaxDepthDFS = 100;
    }
}
