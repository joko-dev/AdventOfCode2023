using AdventOfCode2022.SharedKernel;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 13: Point of Incidence"));
            Console.WriteLine("Patterns: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<List<string>> patterns = getPatterns(puzzleInput.Lines);
            List<int> rowsAbove = new List<int>();
            List<int> columnsLeft = new List<int>();
            foreach(List<string> pattern in patterns)
            {
                rowsAbove.Add(getHorizontalReflection(pattern));
                columnsLeft.Add(getVerticalReflection(pattern));
            }
            Console.WriteLine("Summarize: {0}", columnsLeft.Sum() + (rowsAbove.Sum() * 100));
        }

        private static int getVerticalReflection(List<string> pattern)
        {
            int columnLeft = 0;
            char[,] matrix = PuzzleConverter.getInputAsMatrixChar(pattern, null);

            for (int mirror = matrix.GetLength(0) - 1; mirror >= 0; mirror--)
            {
                bool perfectMirror = true;
                for(int y = 0; y < matrix.GetLength(1); y++)
                {
                    for(int x = mirror - 1; x >= 0; x--)
                    {
                        int mirroredIndex = mirror + (mirror - x - 1);
                        if (mirroredIndex >= matrix.GetLength(0))
                        {
                            break;
                        }

                        if (matrix[x,y] != matrix[mirroredIndex, y])
                        {
                            perfectMirror = false;
                            break;
                        }
                    }
                    if (!perfectMirror)
                    {
                        break;
                    }
                }

                if ( perfectMirror )
                {
                    columnLeft = mirror;
                    break;
                }
            }

            return columnLeft;
        }
        private static int getHorizontalReflection(List<string> pattern)
        {
            int rowsAbove = 0;
            char[,] matrix = PuzzleConverter.getInputAsMatrixChar(pattern, null);

            for (int mirror = matrix.GetLength(1) - 1; mirror >= 0; mirror--)
            {
                bool perfectMirror = true;
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    for (int y = mirror - 1; y >= 0; y--)
                    {
                        int mirroredIndex = mirror + (mirror - y - 1);
                        if (mirroredIndex >= matrix.GetLength(1))
                        {
                            break;
                        }

                        if (matrix[x, y] != matrix[x, mirroredIndex])
                        {
                            perfectMirror = false;
                            break;
                        }
                    }
                    if (!perfectMirror)
                    {
                        break;
                    }
                }

                if (perfectMirror)
                {
                    rowsAbove = mirror;
                    break;
                }
            }

            return rowsAbove;
        }

        private static List<List<string>> getPatterns(List<string> lines)
        {
            List<List<string>> patterns = new List<List<string>>();
            List<string> pattern = new List<string>();

            foreach (string line in lines)
            {
                if(line.Length == 0)
                {
                    patterns.Add(pattern);
                    pattern = new List<string>();
                }
                else
                {
                    pattern.Add(line);
                }
            }
            patterns.Add(pattern);

            return patterns;
        }
    }
}