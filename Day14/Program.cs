using AdventOfCode2022.SharedKernel;
using System.Text;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 14: Parabolic Reflector Dish"));
            Console.WriteLine("Platform: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] platform = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null);
            tiltPlatformNorth(platform);

            Console.WriteLine("Total load: {0}", calculateTotalLoad(platform));
            
            Dictionary<int, int> loopDetection = new Dictionary<int, int>();
            platform = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null);
            int currentCycle = 0;
            int cycles = 1000000000;
            bool loopFound = false;
            while (currentCycle < cycles)
            {
                tiltPlatformNorth(platform);
                tiltPlatformWest(platform);
                tiltPlatformSouth(platform);
                tiltPlatformEast(platform);

                currentCycle++;

                if (!loopFound)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in PuzzleOutputFormatter.outputMap(platform))
                    {
                        sb.Append(line);
                    }
                    int hash = sb.ToString().GetHashCode();

                    if (loopDetection.ContainsKey(hash))
                    {
                        int lastCycle = loopDetection[hash];
                        int remainingCycles = cycles - currentCycle;
                        Math.DivRem(remainingCycles, (currentCycle - lastCycle), out int remain);
                        currentCycle = remainingCycles + currentCycle - remain;
                        loopFound = true;

                    }
                    else
                    {
                        loopDetection[hash] = currentCycle;
                    }
                }
            }
            Console.WriteLine("Total load: {0}", calculateTotalLoad(platform));
        }

        private static void tiltPlatformNorth(char[,] platform)
        {
            for (int x = 0; x < platform.GetLength(0); x++)
            {
                for (int y = 0; y < platform.GetLength(1); y++)
                {
                    if (platform[x, y] == 'O') { slideNorth(platform, new Coordinate(x, y)); }
                    
                }
            }
        }
        private static void tiltPlatformSouth(char[,] platform)
        {
            for (int x = 0; x < platform.GetLength(0); x++)
            {
                for (int y = platform.GetLength(1) - 1; y >= 0; y--)
                {
                    if (platform[x, y] == 'O') { slideSouth(platform, new Coordinate(x, y)); }

                }
            }
        }
        private static void tiltPlatformWest(char[,] platform)
        {
            for (int x = 0; x < platform.GetLength(0); x++)
            {
                for (int y = 0; y < platform.GetLength(1); y++)
                {
                    if (platform[x, y] == 'O') { slideWest(platform, new Coordinate(x, y)); }

                }
            }
        }
        private static void tiltPlatformEast(char[,] platform)
        {
            for (int x = platform.GetLength(0) - 1; x >= 0; x--)
            {
                for (int y = 0; y < platform.GetLength(1); y++)
                {
                    if (platform[x, y] == 'O') { slideEast(platform, new Coordinate(x, y)); }

                }
            }
        }

        private static void slideNorth(char[,] platformTilted, Coordinate coordinate)
        {
            for(int y = coordinate.Y; y >= 1; y--)
            {
                if (platformTilted[coordinate.X, y - 1] == '.')
                {
                    platformTilted[coordinate.X, y - 1] = 'O';
                    platformTilted[coordinate.X, y] = '.';
                }
                else 
                {
                    break;
                }
            }
        }

        private static void slideSouth(char[,] platformTilted, Coordinate coordinate)
        {
            for (int y = coordinate.Y; y <= platformTilted.GetLength(1) - 2; y++)
            {
                if (platformTilted[coordinate.X, y + 1] == '.')
                {
                    platformTilted[coordinate.X, y + 1] = 'O';
                    platformTilted[coordinate.X, y] = '.';
                }
                else
                {
                    break;
                }
            }
        }

        private static void slideWest(char[,] platformTilted, Coordinate coordinate)
        {
            for (int x = coordinate.X; x > 0; x--)
            {
                if (platformTilted[x - 1, coordinate.Y] == '.')
                {
                    platformTilted[x - 1, coordinate.Y] = 'O';
                    platformTilted[x, coordinate.Y] = '.';
                }
                else
                {
                    break;
                }
            }
        }

        private static void slideEast(char[,] platformTilted, Coordinate coordinate)
        {
            for (int x = coordinate.X; x <= platformTilted.GetLength(0) - 2; x++)
            {
                if (platformTilted[x + 1, coordinate.Y] == '.')
                {
                    platformTilted[x + 1, coordinate.Y] = 'O';
                    platformTilted[x, coordinate.Y] = '.';
                }
                else
                {
                    break;
                }
            }
        }

        private static int calculateTotalLoad(char[,] platform)
        {
            int totalLoad = 0;
            for(int y = 0; y < platform.GetLength(1); y++)
            {
                for (int x = 0; x < platform.GetLength(0); x++)
                {
                    if (platform[x, y] == 'O')
                    {
                        totalLoad += platform.GetLength(1) - y;
                    }
                }
            }
            return totalLoad;
        }
    }
}
