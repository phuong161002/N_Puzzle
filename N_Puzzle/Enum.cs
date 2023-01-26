using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Puzzle
{
    public enum MoveDirection
    {
        Up, 
        Down,
        Left,
        Right
    }

    public enum SolverType
    {
        AStar_Manhattan,
        AStar_MisplacedTiles,
        BFS, 
        DFS,
    }

    public enum SolvingStatus
    {
        None,
        Solving,
        Success,
        Failed_OutOfMem,
        Failed_TimeOut,
    }
}
