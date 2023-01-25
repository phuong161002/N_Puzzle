using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N_Puzzle.Algorithms;

namespace N_Puzzle
{
    public class Controller
    {
        private ISolver _solver;
        private MainForm parent;
        private Stopwatch _stopwatch;
        public int[] CurrentState;

        public Controller(MainForm mainForm)
        {
            parent = mainForm;
        }

        public void Solve(SolverType type, int[] startState, int[] goalState)
        {
            if(_solver != null && _solver.Status == SolvingStatus.Solving)
            {
                return;
            }
            Node.Reset();
            _solver = GetSolver(type);
            _solver.OnSolvingCompleted += _solver_OnSolvingCompleted;
            _solver.OnSolvingFailed += _solver_OnSolvingFailed;
            _stopwatch = Stopwatch.StartNew();
            _solver.Solve(startState, goalState);
            _stopwatch.Stop();
        }

        private void _solver_OnSolvingFailed()
        {
            Console.WriteLine($"Solve Failed\n" +
                $"Solving Time : {(int)_stopwatch.Elapsed.TotalMilliseconds}ms\n" +
                $"Num Evaluated Nodes: {Node.NumEvaluatedNodes}");
        }

        private void _solver_OnSolvingCompleted()
        {
            Console.WriteLine("Solved");
            Console.WriteLine($"Solving Time : {(int)_stopwatch.Elapsed.TotalMilliseconds}ms\n" +
                $"Num Evaluated Nodes: {Node.NumEvaluatedNodes}\n" +
                $"Depth: {_solver.GoalNode.depth}");
            var listMove = TraceMove();
            parent.PerformMoves(listMove);
        }

        private ISolver GetSolver(SolverType type)
        {
            switch(type)
            {
                case SolverType.AStar:
                    return new AStar();
                case SolverType.BFS:
                    return new BFS();
                case SolverType.DFS:
                    return new DFS();
                default:
                    return new AStar();
            }
        }

        private List<int[]> TraceMove()
        {
            var currentNode = _solver.GoalNode;
            var listMove = new List<int[]>();
            while(currentNode != null)
            {
                listMove.Add(currentNode.state);

                currentNode = currentNode.parent;
            }

            listMove.Reverse();
            return listMove;
        }
    }
}
