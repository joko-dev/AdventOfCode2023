using AdventOfCode2022.SharedKernel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 01: Trebuchet?!"));
            Console.WriteLine("Calibration document: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int sum = 0;

            foreach(string line in puzzleInput.Lines)
            {
                List<char> digits = line.Where(c => Char.IsDigit(c)).ToList();
                string number = String.Concat(digits.First(), digits.Last());

                sum += int.Parse(number) ;
            }

            Console.WriteLine("Sum calibration values: {0}", sum);
        }
    }
}