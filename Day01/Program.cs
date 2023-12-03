using AdventOfCode2022.SharedKernel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Day01
{
    internal class Program
    {
        static List<string> numberWords = new List<string> {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "zero" };
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 01: Trebuchet?!"));
            Console.WriteLine("Calibration document: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<int> calibrationValues = getCalibrationValues(puzzleInput.Lines, false);
            Console.WriteLine("Sum calibration values: {0}", calibrationValues.Sum());

            calibrationValues = getCalibrationValues(puzzleInput.Lines, true);
            Console.WriteLine("Sum calibration values with words: {0}", calibrationValues.Sum());

        }

        static List<int> getCalibrationValues(List<string> lines, bool withWords)
        {
            List<int> calibrationValues = new List<int>();
            foreach (string line in lines)
            {
                List<char> digits = getDigits(line, withWords) ;
                string number = "";

                if (digits.Count  > 0)
                {
                    number = String.Concat(digits.First(), digits.Last()); ;
                }
                else
                {
                    number = "0";
                }

                calibrationValues.Add(int.Parse(number));
            }
            return calibrationValues;
        }
        static List<char> getDigits(string line, bool withWords)
        {
            List<char> digits = new List<char>();
            string number = "";
            int i = 0;
            int firstIndexWord = 0;
            while (i < line.Length)
            {
                
                if (Char.IsDigit(line[i]))
                {
                    digits.Add(line[i]);
                    number = "";
                }
                else if(withWords)
                {
                    
                    number = String.Concat(number, line[i]);

                    if (number.Length == 1)
                    {
                        firstIndexWord = i;
                    }

                    if (numberWords.Contains(number))
                    {
                        switch (number)
                        {
                            case "one": digits.Add('1'); break;
                            case "two": digits.Add('2'); break;
                            case "three": digits.Add('3'); break;
                            case "four": digits.Add('4'); break;
                            case "five": digits.Add('5'); break;
                            case "six": digits.Add('6'); break;
                            case "seven": digits.Add('7'); break;
                            case "eight": digits.Add('8'); break;
                            case "nine": digits.Add('9'); break;
                            case "zero": digits.Add('0'); break;
                        }
                        i = firstIndexWord;
                        number = "";
                    }
                    else if(!numberWords.Any(w => w.StartsWith(number)))
                    {
                        number = "";
                        i = firstIndexWord;
                    }
                }
                i++;
            }
           
            return digits;
        }

    }
}