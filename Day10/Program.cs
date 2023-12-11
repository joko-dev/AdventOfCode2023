using AdventOfCode2022.SharedKernel;
using System.Reflection.Metadata;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 10: Pipe Maze"));
            Console.WriteLine("Pipes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] map = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null);
            List<Coordinate> coordinates = getLoop(map);
            Console.WriteLine("Farthest point from start: {0}", coordinates.Count / 2);


            // ToDo: First idea flood fill, but squeezing could be difficult
        }

        private static List<Coordinate> getLoop(char[,] map)
        {
            List<Coordinate> loop = new();
            bool loopFinished = false;
            Coordinate coordinate = findCoordinate(map, 'S');

            loop.Add(coordinate);
            do
            {
                Coordinate next = traverseLoop(loop.Last(), loop.SkipLast(1).LastOrDefault(), map);
                if (map[next.X, next.Y] != 'S')
                {
                    loop.Add(next);
                }
                else
                {
                    loopFinished = true;
                }
            }
            while (!loopFinished);


            return loop;
        }

        private static Coordinate traverseLoop(Coordinate current, Coordinate predecessor, char[,] map)
        {
            char pipe = map[current.X, current.Y];
            List<Coordinate> adjacent = new List<Coordinate>();
            if(pipe == '|')
            {
               adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y)) current, false, false, true, true).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == '-')
            {
                adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, true, true, false, false).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == 'L')
            {
                adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, false, true, true, false).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == 'J')
            {
                adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, true, false, true, false).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == '7')
            {
                adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, true, false, false, true).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == 'F')
            {
                adjacent = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, false, true, false, true).Select(a => new Coordinate(a)).ToList();
            }
            else if (pipe == 'S')
            {
                Coordinate? left = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, true, false, false, false).Select(a => new Coordinate(a)).FirstOrDefault();
                Coordinate? right = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, false, true, false, false).Select(a => new Coordinate(a)).FirstOrDefault();
                Coordinate? up = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, false, false, true, false).Select(a => new Coordinate(a)).FirstOrDefault();
                Coordinate? down = PuzzleConverter.getAdjacentPoints(map, ((int x, int y))current, false, false, false, true).Select(a => new Coordinate(a)).FirstOrDefault();

                if (left is not null && (map[left.X, left.Y] == '-' || map[left.X, left.Y] == 'L' || map[left.X, left.Y] == 'F')) { adjacent.Add(left); }
                if (right is not null && (map[right.X, right.Y] == '-' || map[right.X, right.Y] == '7' || map[right.X, right.Y] == 'J')) { adjacent.Add(right); }
                if (up is not null && (map[up.X, up.Y] == '|' || map[up.X, up.Y] == '7' || map[up.X, up.Y] == 'F')) { adjacent.Add(up); }
                if (down is not null && (map[down.X, down.Y] == '|' || map[down.X, down.Y] == 'J' || map[down.X, down.Y] == 'L')) { adjacent.Add(down); }
                
            }

            if(adjacent.Count != 2)
            {
                throw new Exception();
            }

            if (predecessor == null) { return adjacent.First(); }
            else { return adjacent.Where( a=> !a.Equals(predecessor)).First(); }
            
        }

        private static Coordinate findCoordinate(char[,] map, char value)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == value)
                    {
                        return new Coordinate(x, y);
                    }
                }
            }
            return null;
        }
    }
}