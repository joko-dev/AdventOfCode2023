using AdventOfCode2022.SharedKernel;
using System.Collections.Generic;

namespace Day02
{
    internal class Program
    {
        static List<Cubes> cubeSpecification = new List<Cubes> { new Cubes("red", 12), new Cubes("green", 13), new Cubes("blue", 14) } ;
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 2: Cube Conundrum"));
            Console.WriteLine("Game record: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int sumIds = 0;
            int sumPower = 0;

            for (int i = 0; i < puzzleInput.Lines.Count; i++)
            {
                if (gameIsPossible(puzzleInput.Lines[i]))
                {
                    sumIds += (i + 1); 
                }
            }
            Console.WriteLine("Sum of GameID: {0}", sumIds);

            foreach(string line in puzzleInput.Lines)
            {
                sumPower += getGamePower(line);
            }
            Console.WriteLine("Sum of power: {0}", sumPower);
        }

        private static int getGamePower(string line)
        {
            int power = 1;
            List<Cubes> cubesPlay = getCubeList(line);

            foreach(Cubes cube in cubeSpecification)
            {
                power = power * cubesPlay.Where(c => c.Color == cube.Color).Max(c => c.Count);
            }

            return power;
        }

        private static bool gameIsPossible(string line)
        {
            bool possible = false;
            List < Cubes > cubesPlay = getCubeList(line);

            possible = !(cubesPlay.Any(c => cubeSpecification.Any(s => s.Color == c.Color && (c.Count > s.Count)) ));

            return possible;
        }

        private static List<Cubes> getCubeList(string line)
        {
            List<Cubes>  cubesPlay = new List<Cubes>();
            bool possible = false;
            line = line.Remove(0, line.IndexOf(':') + 2);
            line = line.Replace(";", "").Replace(",", "");

            string[] elements = line.Split(" ");

            for (int i = 0; i < elements.Length; i = i + 2)
            {
                cubesPlay.Add(new Cubes(elements[i + 1], Int32.Parse(elements[i])));
            }

            return cubesPlay;
        }
    }
}