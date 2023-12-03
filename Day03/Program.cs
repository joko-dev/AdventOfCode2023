using AdventOfCode2022.SharedKernel;
using System.Drawing;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 3: Gear Ratios"));
            Console.WriteLine("Engine schematic: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<int> partNumbers = getPartNumbers(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            Console.WriteLine("Sum of Part numbers: {0}", partNumbers.Sum());

            List<int> gearRatios = getGearRatios(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            Console.WriteLine("Sum of gear ratios: {0}", gearRatios.Sum());
        }

        private static List<int> getGearRatios(char[,] engine)
        {
            List<int> gearRatios = new();

            for (int y = 0; y < engine.GetLength(1); y++)
            {
                for (int x = 0; x < engine.GetLength(0); x++)
                {
                    if (engine[x,y] == '*')
                    {
                        List<Coordinate> scannedStarts = new List<Coordinate>();
                        List<int> parts = new List<int>();
                        List<(int x, int y)> adjacent = PuzzleConverter.getAdjacentPoints(engine, (x, y), true, true, true);
                        foreach((int x, int y) adj in adjacent)
                        {
                            if (Char.IsDigit(engine[adj.x, adj.y]))
                            {
                                Coordinate starting = getStartingCoordinate(engine, new Coordinate(adj));
                                if(!scannedStarts.Exists(s => s.Equals(starting)))
                                {
                                    parts.Add(getFullPartNumber(engine, starting));
                                    scannedStarts.Add(starting);
                                }
                            }
                            
                        }
                        if(parts.Count == 2)
                        {
                            gearRatios.Add(parts[0] * parts[1]);
                        }
                    }
                }
            }

            return gearRatios;
        }

        private static Coordinate getStartingCoordinate(char[,] engine, Coordinate point)
        {
            int startX = 0;

            for (int x = point.X; x >= 0; x--)
            {
                if (Char.IsDigit(engine[x, point.Y]))
                {
                    startX = x;
                }
                else
                {
                    break;
                }
            }

            return new Coordinate(startX, point.Y);
        }

        private static int getFullPartNumber(char[,] engine, Coordinate startPoint)
        {
            string part = "";
            for (int x = startPoint.X; x < engine.GetLength(0); x++)
            {
                if (Char.IsDigit(engine[x, startPoint.Y]))
                {
                    part += engine[x, startPoint.Y];
                }
                else
                {
                    break;
                }
            }
            return Int32.Parse(part);
        }

        private static List<int> getPartNumbers(char[,] engine)
        {
            List<int> partNumbers = new();

            for(int y= 0; y < engine.GetLength(1); y++)
            {
                int startX = -1;
                int endX = -1;

                for ( int x= 0; x < engine.GetLength(0); x++)
                {
                    if (Char.IsDigit(engine[x, y]))
                    {
                        if (startX < 0) { startX = x; }
                        endX = x;
                    }
                    if ((startX > -1 ) && (!Char.IsDigit(engine[x, y]) || (x == engine.GetLength(0) - 1)))
                    {
                        string partNumber = "";
                        bool foundSymbol = false;
                        for(int xPart = startX; xPart <= endX; xPart++)
                        {
                            partNumber += engine[xPart, y];

                            List<(int x, int y)> adjacent = PuzzleConverter.getAdjacentPoints(engine, (xPart, y), true, true, true);
                            foreach((int x, int y) adj in adjacent)
                            {
                                char c = engine[adj.x, adj.y];
                                if ( !Char.IsDigit(c) && c != '.')
                                {
                                    foundSymbol = true;
                                    break;
                                }
                            }
                        }

                        if (foundSymbol)    
                        {
                            partNumbers.Add(Int32.Parse(partNumber));
                        }

                        startX = -1;
                        endX = -1;
                    }
                }
            }

            return partNumbers;
        }

    }
}