using AdventOfCode2022.SharedKernel;
using SharedKernel;
using System.Runtime.CompilerServices;

namespace Day17
{
    internal class Program
    {
        static List<Coordinate> coordinates = new List<Coordinate>();
        private readonly record struct State(Move move, int directionCount);
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 17: Clumsy Crucible"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int[,] heatMap = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                for (int y = 0; y < heatMap.GetLength(1); y++)
                {
                    coordinates.Add(new Coordinate(x, y));
                }
            }
            Console.WriteLine("Least heat loss: {0}", TraverseMap(heatMap, 1, 3));
            Console.WriteLine("Least heat loss ultra: {0}", TraverseMap(heatMap, 4, 10));
        }

        static int TraverseMap(int[,] heatMap, int minMoves, int maxMoves)
        {
            State startRight= new State(move: new Move(new Coordinate(0, 0), Move.DirectionType.Right), directionCount: 0);
            State startDown = new State(move: new Move(new Coordinate(0, 0), Move.DirectionType.Down), directionCount: 0);
            Coordinate ending = new Coordinate(heatMap.GetLength(0) - 1, heatMap.GetLength(1) - 1);

            DefaultDictionary<State, int> cost = new DefaultDictionary<State, int>() { defaultValue = int.MaxValue / 2};
            cost.Add(startRight, 0);;
            cost.Add(startDown, 0);;
            PriorityQueue<State, int> heap = new PriorityQueue<State, int>();
            heap.Enqueue(startRight, 0);
            heap.Enqueue(startDown, 0);

            while (heap.Count > 0)
            {
                State state = heap.Dequeue();
                if(state.move.Coordinate.Equals(ending) && state.directionCount >= minMoves) 
                {
                    return cost[state];
                }

                foreach (var adjacent in GetAdjacentStates(state, minMoves, maxMoves))
                {
                    if(PuzzleConverter.isPointInMatrix(heatMap, ((Int64 x, Int64 y))adjacent.move.Coordinate) && (cost[state] + heatMap[adjacent.move.Coordinate.X, adjacent.move.Coordinate.Y] < cost[adjacent]))
                    {
                        cost[adjacent] = cost[state] + heatMap[adjacent.move.Coordinate.X, adjacent.move.Coordinate.Y];
                        heap.Enqueue(adjacent, cost[adjacent]);
                    }
                }
            }

            throw new NotImplementedException();
        }

        private static IEnumerable<State> GetAdjacentStates(State state, int minMoves, int maxMoves)
        {
            if(state.directionCount >= minMoves)
            {
                State rotatedLeft = new State(move: state.move.RotateLeft().MoveToDirection(), directionCount: 1);
                State rotatedRight = new State(move: state.move.RotateRight().MoveToDirection(), directionCount: 1);
                yield return rotatedLeft;
                yield return rotatedRight;
            }
            if(state.directionCount < maxMoves)
            {
                Move forward = state.move.MoveToDirection();
                yield return new State(forward, state.directionCount+1);
            }
        }
    }
}
