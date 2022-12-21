using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using N_Puzzle.Properties;

namespace N_Puzzle
{
    public static class Util
    {
        public static bool IsGoalState(Node node, Node goalNode)
        {
            var state = node.state;
            var goalState = goalNode.state;
            for (int i = 0; i < state.Length && i < goalState.Length; i++)
            {
                if(state[i] != goalState[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryMove(Node currentNode, MoveDirection direction, out Node nextNode)
        {
            Node node = Move(currentNode, direction);
            if (node != null)
            {
                nextNode = node;
                return true;
            }

            // Can not move
            nextNode = null;
            return false;
        }

        public static Node Move(Node currentNode, MoveDirection direction)
        {
            int[] currentState = (int[])currentNode.state.Clone();
            int[] newState;
            switch (direction)
            {
                case MoveDirection.Up:
                    newState = MoveUp(currentState);
                    break;
                case MoveDirection.Down:
                    newState = MoveDown(currentState);
                    break;
                case MoveDirection.Left:
                    newState = MoveLeft(currentState);
                    break;
                case MoveDirection.Right:
                    newState = MoveRight(currentState);
                    break;
                default:
                    newState = null;
                    break;
            }

            // Can not move
            if (newState == null)
            {
                return null;
            }

            Node nextNode = new Node(newState)
            {
                parent = currentNode,
                depth = currentNode.depth + 1
            };

            currentNode.AddChild(nextNode);

            return nextNode;
        }

        private static int[] Move(int[] state, MoveDirection direction)
        {
            int[] newState;
            switch (direction)
            {
                case MoveDirection.Up:
                    newState = MoveUp(state);
                    break;
                case MoveDirection.Down:
                    newState = MoveDown(state);
                    break;
                case MoveDirection.Left:
                    newState = MoveLeft(state);
                    break;
                case MoveDirection.Right:
                    newState = MoveRight(state);
                    break;
                default:
                    newState = null;
                    break;
            }

            return newState;
        }

        private static int[] MoveUp(int[] state)
        {
            int x, y;
            (x, y) = FindZeroPos(state);
            if (y <= 0)
            {
                return null;
            }
            int oldPos = y * Settings.SIZE + x;
            int newPos = (y - 1) * Settings.SIZE + x;
            SwapInt(ref state[oldPos], ref state[newPos]);
            return state;
        }

        private static int[] MoveDown(int[] state)
        {
            int x, y;
            (x, y) = FindZeroPos(state);
            if (y >= Settings.SIZE - 1)
            {
                return null;
            }
            int oldPos = y * Settings.SIZE + x;
            int newPos = (y + 1) * Settings.SIZE + x;
            SwapInt(ref state[oldPos], ref state[newPos]);
            return state;
        }

        private static int[] MoveLeft(int[] state)
        {
            int x, y;
            (x, y) = FindZeroPos(state);
            if (x == 0)
            {
                return null;
            }
            int oldPos = y * Settings.SIZE + x;
            int newPos = y * Settings.SIZE + x - 1;
            SwapInt(ref state[oldPos], ref state[newPos]);
            return state;
        }

        private static int[] MoveRight(int[] state)
        {
            int x, y;
            (x, y) = FindZeroPos(state);
            if (x == Settings.SIZE - 1)
            {
                return null;
            }
            int oldPos = y * Settings.SIZE + x;
            int newPos = y * Settings.SIZE + x + 1;
            SwapInt(ref state[oldPos], ref state[newPos]);
            return state;
        }

        public static void SwapInt(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static (int, int) FindZeroPos(int[] state)
        {
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == 0)
                {
                    return (i % Settings.SIZE, i / Settings.SIZE);
                }
            }
            return (-1, -1);
        }

        public static bool IsValidState(int[] state)
        {
            if (state.Length != Settings.SIZE * Settings.SIZE)
            {
                return false;
            }

            int countZero = 0;

            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == 0)
                {
                    countZero++;
                    if (countZero > 1)
                    {
                        return false;
                    }
                }
            }

            return countZero == 1;
        }

        public static void Print(int[] state, string stateName = "")
        {
            if (state == null)
            {
                return;
            }

            Console.WriteLine($"================{stateName}=================");
            for (int i = 0; i < state.Length; i++)
            {
                Console.Write(state[i] + " ");
                if (i % Settings.SIZE == Settings.SIZE - 1)
                {
                    Console.WriteLine();
                }
            }
        }

        public static int[] Shuffer(int[] state, int iterations)
        {
            Random random = new Random();
            int[] currenState = (int[])state.Clone();
            for (int i = 0; i < iterations; i++)
            {
                int num = random.Next(0, 4);
                var newState = Move(currenState, (MoveDirection)num);
                if (newState != null)
                {
                    currenState = newState;
                }
            }

            return currenState;
        }

        public static List<Node> Trace(Node node)
        {
            List<Node> result = new List<Node>();
            Node currentNode = node;
            result.Add(node);
            while(currentNode.parent != null)
            {
                result.Add(currentNode.parent);
                currentNode = currentNode.parent;
            }

            result.Reverse();

            return result;
        }
    }
}
