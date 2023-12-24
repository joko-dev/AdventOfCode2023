using AdventOfCode2022.SharedKernel;
using System.ComponentModel;
using System.Numerics;
using System.Windows;
using System.Runtime.InteropServices;

namespace Day18
{
    internal class Program
    {
        private readonly record struct DigSpec (Move.DirectionType Direction, int Distance);
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 18: Lavaduct Lagoon"));
            Console.WriteLine("Dig plan: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<DigSpec> digSpec = getDiggingSpecification(puzzleInput.Lines, false);
            List<Coordinate> trench = buildTrench(digSpec);
            List<Coordinate> diggedOut = digOutInterior(trench);
            Console.WriteLine("Cubic meters (Flood Fill): {0}", diggedOut.Count);
            trench = getTrenchTrapez(digSpec);
            Console.WriteLine("Cubic meters (Gauss + Pick): {0}", getAreaShoelaceGauss(trench, digSpec.Sum(s => s.Distance)));

            digSpec = getDiggingSpecification(puzzleInput.Lines, true);
            trench = getTrenchTrapez(digSpec);
            Console.WriteLine("Hexadecimal area (Gauss + Pick): {0}", getAreaShoelaceGauss(trench, digSpec.Sum(s => s.Distance)));
        }

        private static Int64 getAreaShoelaceGauss(List<Coordinate> trench, Int64 trenchLength)
        {
            Int64 minX = trench.Min(c => c.X);
            Int64 minY = trench.Min(c => c.Y);
            if (minX >= 0) { minX = -1; }
            if (minY >= 0) { minY = -1; }

            foreach (Coordinate c in trench)
            {
                c.Move(-minX, -minY);
            }

            //  Gauss's area f
            Int64 A2 = 0;
            for(int i = 0; i < trench.Count() - 1; i++)
            {
                A2 += (trench[i].X * trench[i+1].Y) - (trench[i].Y * trench[i + 1].X);
            }
            A2 += (trench.Last().X * trench[0].Y) - (trench.Last().Y * trench[0].X);

            Int64 A = (A2 / 2);
            // Pick's theorem
            A += (trenchLength / 2 + 1);
            return A;
        }

        private static List<Coordinate> digOutInterior(List<Coordinate> trench)
        {
            
            Coordinate minCoordinate = new Coordinate(trench.Min(c => c.X), trench.Min(c => c.Y));
            Coordinate maxCoordinate = new Coordinate(trench.Max(c => c.X), trench.Max(c => c.Y));

            // since we dont know the circle direction we try different start points
            try
            {
                List<Coordinate> result = trench.ToList();
                Coordinate start = new Coordinate(result.First().X - 1, result.First().Y - 1);
                FloodFill(start, result, minCoordinate, maxCoordinate);
                return result;
            }
            catch (InvalidCoordinateException e) { }

            try
            {
                List<Coordinate> result = trench.ToList();
                Coordinate start = new Coordinate(result.First().X - 1, result.First().Y + 1);
                FloodFill(start, result, minCoordinate, maxCoordinate);
                return result;
            }
            catch (InvalidCoordinateException e) { }

            try
            {
                List<Coordinate> result = trench.ToList();
                Coordinate start = new Coordinate(result.First().X + 1, result.First().Y - 1);
                FloodFill(start, result, minCoordinate, maxCoordinate);
                return result;
            }
            catch (InvalidCoordinateException e) { }

            try
            {
                List<Coordinate> result = trench.ToList();
                Coordinate start = new Coordinate(result.First().X + 1, result.First().Y + 1);
                FloodFill(start, result, minCoordinate, maxCoordinate);
                return result;
            }
            catch (InvalidCoordinateException e) { }

            throw new ArgumentException();
        }
        private static void FloodFill(Coordinate start, List<Coordinate> trench, Coordinate min, Coordinate max)
        {
            //Iterativ floodfill, recursice would lead to stack trace since we have to try different starting points
            Stack<Coordinate> stack = new Stack<Coordinate>();
            stack.Push(start);

            while(stack.Count > 0)
            {
                Coordinate coordinate = stack.Pop();

                // coordinate is out of our min and max range --> starting point was not inside the trench
                if (coordinate.X < min.X || coordinate.Y < min.Y || coordinate.X > max.X || coordinate.Y > max.Y)
                {
                    throw new InvalidCoordinateException();
                }

                if (!trench.Contains(coordinate))
                {
                    trench.Add(coordinate);
                    stack.Push(coordinate.GetAdjacentCoordinate(Move.DirectionType.Left));
                    stack.Push(coordinate.GetAdjacentCoordinate(Move.DirectionType.Right));
                    stack.Push(coordinate.GetAdjacentCoordinate(Move.DirectionType.Up));
                    stack.Push(coordinate.GetAdjacentCoordinate(Move.DirectionType.Down));
                }
            }
        }

        private static List<Coordinate> buildTrench(List<DigSpec> spec)
        {
            List<Coordinate> result = new List<Coordinate>();
            result.Add(new Coordinate(0, 0));

            foreach (DigSpec digSpec in spec)
            {
                for (int i = 1; i <= digSpec.Distance; i++)
                {
                    int xToAdd = 0;
                    int yToAdd = 0;
                    switch (digSpec.Direction) 
                    {
                        case Move.DirectionType.Left: xToAdd--; break;
                        case Move.DirectionType.Right: xToAdd++; break;
                        case Move.DirectionType.Up: yToAdd--; break;
                        case Move.DirectionType.Down: yToAdd++; break;
                    }

                    Coordinate coordinate = new Coordinate(result.Last().X + xToAdd, result.Last().Y + yToAdd);
                    if (!result.Contains(coordinate)) 
                    {
                        result.Add(coordinate);
                    }
                    
                }
            }

            return result;
        }
        private static List<Coordinate> getTrenchTrapez(List<DigSpec> spec)
        {
            List<Coordinate> result = new List<Coordinate>();
            result.Add(new Coordinate(0, 0));

            foreach (DigSpec digSpec in spec)
            {
                int xToAdd = 0;
                int yToAdd = 0;
                switch (digSpec.Direction)
                {
                    case Move.DirectionType.Left: xToAdd -= digSpec.Distance; break;
                    case Move.DirectionType.Right: xToAdd += digSpec.Distance; break;
                    case Move.DirectionType.Up: yToAdd -= digSpec.Distance; break;
                    case Move.DirectionType.Down: yToAdd += digSpec.Distance; break;
                }

                Coordinate coordinate = new Coordinate(result.Last().X + xToAdd, result.Last().Y + yToAdd);
                if (!result.Contains(coordinate))
                {
                    result.Add(coordinate);
                }
            }

            return result;
        }

        private static List<DigSpec> getDiggingSpecification(List<string> lines, bool colorAsInstruction)
        {
            List<DigSpec> specs = new List<DigSpec>();
            foreach (string line in lines)
            {
                string[] strings = line.Split(' ');
                int distance = 0;
                char directionInstruction;
                Move.DirectionType direction;
                
                if (colorAsInstruction)
                {
                    string color = strings.Last().Replace("(", "").Replace(")", "").Replace("#", "");
                    distance = Convert.ToInt32(color.Substring(0,5), 16);
                    directionInstruction = color[5];
                    switch(directionInstruction)
                    {
                        case '0':  directionInstruction = 'R'; break;
                        case '1':  directionInstruction = 'D'; break;
                        case '2':  directionInstruction = 'L'; break;
                        case '3':  directionInstruction = 'U'; break;
                    } 
                }
                else
                {
                    distance = int.Parse(strings[1]);
                    directionInstruction = strings.First().First();
                }

                switch (directionInstruction)
                {
                    case 'R': { direction = Move.DirectionType.Right; break; }
                    case 'L': { direction = Move.DirectionType.Left; break; }
                    case 'U': { direction = Move.DirectionType.Up; break; }
                    case 'D': { direction = Move.DirectionType.Down; break; }
                    default: { throw new ArgumentException(); }
                }

                specs.Add(new DigSpec(Direction: direction, distance));
            }
            return specs;
        }
    }
}
