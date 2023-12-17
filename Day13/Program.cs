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
                char[,] matrix = PuzzleConverter.getInputAsMatrixChar(pattern, null);
                getReflections( matrix, out int above, out int left, 0, 0);
                rowsAbove.Add(above);
                columnsLeft.Add(left);
            }
            Console.WriteLine("Summarize: {0}", columnsLeft.Sum() + (rowsAbove.Sum() * 100));

            rowsAbove.Clear();
            columnsLeft.Clear();
            foreach (List<string> pattern in patterns)
            {
                bool fixedSmudge = false;
                char[,] matrix = PuzzleConverter.getInputAsMatrixChar(pattern, null);
                getReflections(matrix, out int originalAbove, out int originalLeft, 0, 0);

                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        char[,] matrixSmudge = matrix.Clone() as char[,];
                        if (matrixSmudge[x,y] == '#') { matrixSmudge[x, y] = '.'; }
                        else { matrixSmudge[x, y] = '#'; }

                        if(getReflections(matrixSmudge, out int above, out int left, originalAbove, originalLeft))
                        {
                            rowsAbove.Add(above);
                            columnsLeft.Add(left);
                            fixedSmudge = true;
                            break;
                        }
                    }
                    if (fixedSmudge) { break; }
                }
            }
            Console.WriteLine("Summarize: {0}", columnsLeft.Sum() + (rowsAbove.Sum() * 100));
        }

        private static bool getReflections(char[,] matrix, out int rowsAbove, out int columnsLeft, int originalLineAbove, int originalLineLeft)
        {
            columnsLeft = 0;
            rowsAbove = getHorizontalReflection(matrix, originalLineAbove);

            if(rowsAbove == 0)
            {
                columnsLeft = getVerticalReflection(matrix, originalLineLeft);
            }          

            if (rowsAbove > 0 || columnsLeft > 0 )
            {
                return true;
            }
                
            return false;
        }

        private static int getVerticalReflection(char[,] pattern, int original)
        {
            int columnLeft = 0;

            for (int mirror = pattern.GetLength(0) - 1; mirror >= 0; mirror--)
            {
                bool perfectMirror = true;
                for(int y = 0; y < pattern.GetLength(1); y++)
                {
                    for(int x = mirror - 1; x >= 0; x--)
                    {
                        int mirroredIndex = mirror + (mirror - x - 1);
                        if (mirroredIndex >= pattern.GetLength(0))
                        {
                            break;
                        }

                        if (pattern[x,y] != pattern[mirroredIndex, y])
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

                if ( perfectMirror && (mirror != original))
                {
                    columnLeft = mirror;
                    break;
                }
            }

            return columnLeft;
        }
        private static int getHorizontalReflection(char[,] pattern, int original)
        {
            int rowsAbove = 0;

            for (int mirror = pattern.GetLength(1) - 1; mirror >= 0; mirror--)
            {
                bool perfectMirror = true;
                for (int x = 0; x < pattern.GetLength(0); x++)
                {
                    for (int y = mirror - 1; y >= 0; y--)
                    {
                        int mirroredIndex = mirror + (mirror - y - 1);
                        if (mirroredIndex >= pattern.GetLength(1))
                        {
                            break;
                        }

                        if (pattern[x, y] != pattern[x, mirroredIndex])
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

                if (perfectMirror && (mirror != original))
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