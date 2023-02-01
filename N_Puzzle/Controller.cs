﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N_Puzzle.Algorithms;
using System.Threading;

namespace N_Puzzle
{
    public class Controller
    {
        private ISolver _solver;
        private MainForm parent;
        private Stopwatch _stopwatch;
        private Thread solvingThread;
        public int[] CurrentState;
        private Action callbackSolving;

        public Controller(MainForm mainForm)
        {
            parent = mainForm;
        }

        public void Solve(SolverType type, int[] startState, int[] goalState, Action callback = null)
        {
            if (_solver != null && _solver.Status == SolvingStatus.Solving)
            {
                return;
            }
            Node.Reset();
            callbackSolving = callback;
            _solver = GetSolver(type);
            _solver.OnSolvingCompleted += _solver_OnSolvingCompleted;
            _solver.OnSolvingFailed += _solver_OnSolvingFailed;
            solvingThread = new Thread(() =>
            {
                parent.Log("Solving...");
                _stopwatch = Stopwatch.StartNew();
                _solver.Solve(startState, goalState);
                _stopwatch.Stop();
                _solver = null;
                GC.Collect();
                callback?.Invoke();
                callbackSolving = null;
            })
            { IsBackground = true };
            solvingThread.Start();
        }

        private void _solver_OnSolvingFailed()
        {
            parent.Log($"Can not solve!\n" +
                $"Solving Time : {_solver.SolvingTime}ms\n" +
                $"Num Evaluated Nodes: {Node.NodesAlreadyEvaluated}\n" +
                $"Num Nodes in Tree: {Node.NodesInTree}");
        }

        private void _solver_OnSolvingCompleted()
        {
            parent.Log($"Solved!\n" +
                $"Solving Time : {_solver.SolvingTime}ms\n" +
                $"Nodes already evaluated: {Node.NodesAlreadyEvaluated}\n" +
                //$"Num Generated Nodes: {Node.NumGeneratedNode}\n" +
                $"Nodes in Tree: {Node.NodesInTree}\n" +
                $"Depth: {_solver.GoalNode.depth}");
            var listMove = TraceMove();
            parent.PerformMoves(listMove);
        }

        private ISolver GetSolver(SolverType type)
        {
            switch (type)
            {
                case SolverType.AStar_Manhattan:
                    Settings.TypeHeuristic = 0;
                    return new AStar();
                case SolverType.AStar_MisplacedTiles:
                    Settings.TypeHeuristic = 1;
                    return new AStar();
                case SolverType.BFS:
                    return new BFS();
                case SolverType.DFS:
                    return new DFS();
                case SolverType.DLS:
                    return new DLS();
                default:
                    return new AStar();
            }
        }

        private List<int[]> TraceMove()
        {
            var currentNode = _solver.GoalNode;
            var listMove = new List<int[]>();
            while (currentNode != null)
            {
                listMove.Add(currentNode.state);

                currentNode = currentNode.parent;
            }

            listMove.Reverse();
            return listMove;
        }

        public void StopSolving()
        {
            if (solvingThread != null)
            {
                solvingThread.Abort();
                callbackSolving?.Invoke();
                callbackSolving = null;
                solvingThread = null;
                _solver = null;
                _stopwatch = null;
                GC.Collect();
            }
        }
    }
}
