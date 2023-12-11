using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 11: Cosmic Expansion"));
            Console.WriteLine("Pipes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] galaxyMap = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null);
            List<int> emptyRows = getEmptyRows(galaxyMap);
            List<int> emptyColumns = getEmptyColumns(galaxyMap);
            List<(Coordinate, Coordinate)> galaxyPairs = getGalaxyPairs(galaxyMap);
            List<Int64> lengths = determineShortestPaths(galaxyPairs, emptyRows, emptyColumns, 2);
            Console.WriteLine("Sum of lengthts: {0}", lengths.Sum());

            lengths = determineShortestPaths(galaxyPairs, emptyRows, emptyColumns, 1000000);
            Console.WriteLine("Sum of lengthts: {0}", lengths.Sum());
        }

        private static List<(Coordinate, Coordinate)> getGalaxyPairs(char[,] galaxyMap)
        {
            List<Coordinate> galaxies = new List<Coordinate>();
            List<(Coordinate, Coordinate)> pairs = new List<(Coordinate, Coordinate)>();

            for (int y = 0; y < galaxyMap.GetLength(1); y++)
            {
                for (int x = 0; x < galaxyMap.GetLength(0); x++)
                {
                    if (galaxyMap[x, y] == '#') { galaxies.Add(new Coordinate(x, y)); }
                }
            }

            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    pairs.Add((galaxies[i], galaxies[j]));
                }
            }

            return pairs;
        }

        private static List<int> getEmptyRows(char[,] galaxyMap)
        {
            List<int> emptyRows = new List<int>();
            for(int y = 0; y < galaxyMap.GetLength(1); y++)
            {
                bool foundGalaxy = false;
                for (int x = 0; x < galaxyMap.GetLength(0); x++)
                {
                    if (galaxyMap[x,y] == '#') { foundGalaxy = true; break; }
                }
                if (!foundGalaxy) { emptyRows.Add(y); }
            }
            return emptyRows;
        }

        private static List<int> getEmptyColumns(char[,] galaxyMap)
        {
            List<int> emptyColumns = new List<int>();
            for (int x = 0; x < galaxyMap.GetLength(0); x++)
            {
                bool foundGalaxy = false;
                for (int y = 0; y < galaxyMap.GetLength(1); y++)
                {
                    if (galaxyMap[x, y] == '#') { foundGalaxy = true; break; }
                }
                if (!foundGalaxy) { emptyColumns.Add(x); }
            }
            return emptyColumns;
        }

        private static List<Int64> determineShortestPaths(List<(Coordinate, Coordinate)> galaxyPairs, List<int> emptyRows, List<int> emptyColumns, int galaxyEnlargement)
        {
            List<Int64> lengths = new List<Int64>();

            //since we can only go horizontal + vertical and not diagonal we can just add the differences from both axis
            foreach (var pair in galaxyPairs)
            {
                Int64 maxX = Math.Max(pair.Item1.X, pair.Item2.X);
                Int64 minX = Math.Min(pair.Item1.X, pair.Item2.X);
                Int64 maxY = Math.Max(pair.Item1.Y, pair.Item2.Y);
                Int64 minY = Math.Min(pair.Item1.Y, pair.Item2.Y);
                Int64 shortest = maxX - minX + maxY - minY;

                foreach(Int64 row in emptyRows)
                {
                    if(row > minY && row < maxY) { shortest+= galaxyEnlargement - 1; }
                }
                foreach (Int64 column in emptyColumns)
                {
                    if (column > minX && column < maxX) { shortest+= galaxyEnlargement - 1; }
                }
                lengths.Add(shortest);

            }
            return lengths;
        }
    }
}