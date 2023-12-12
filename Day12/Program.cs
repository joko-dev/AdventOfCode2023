using AdventOfCode2022.SharedKernel;
using System.Text;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 12: Hot Springs"));
            Console.WriteLine("Condition records: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int sumPossibleArrangements = 0;

            foreach(string line in puzzleInput.Lines)
            {
                string[] temp = line.Split(' ');
                List<int> group = temp[1].Split(',').Select(int.Parse).ToList();
                sumPossibleArrangements += gePossibleArrangements(temp[0], group);
            }
            Console.WriteLine("Sum of possible arrangements: {0}", sumPossibleArrangements);

            sumPossibleArrangements = 0;
            foreach (string line in puzzleInput.Lines)
            {
                string[] temp = line.Split(' ');
                List<int> group = temp[1].Split(',').Select(int.Parse).ToList();

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(temp[0]);
                for(int i = 1; i <= 4; i++)
                {
                    stringBuilder.Append('?');
                    stringBuilder.Append(temp[0]);
                }

                List<int> groupFiveTimes = new List<int>();
                for(int i = 1; i <= 5; i++) { groupFiveTimes.AddRange(group); }
                sumPossibleArrangements += gePossibleArrangements(stringBuilder.ToString(), groupFiveTimes);
            }
            Console.WriteLine("Sum of possible arrangements: {0}", sumPossibleArrangements);
        }

        private static int gePossibleArrangements(string arrangement, List<int> group)
        {
            //Bruteforcing should be possible for part 1. Better solution idea: Filling from left, when solution not possible we can end early. 
            //Reddit-Hint: Memoization
            int possibleArrangements = 0;

            if (arrangement.Contains('?'))
            {
                if(!validateArrangement(arrangement, group))
                {
                    return 0;
                }

                int firstQuestionmark = arrangement.IndexOf('?');

                StringBuilder stringBuilder = new StringBuilder(arrangement);

                stringBuilder[firstQuestionmark] = '#';
                possibleArrangements += gePossibleArrangements(stringBuilder.ToString(), group);

                stringBuilder[firstQuestionmark] = '.';
                possibleArrangements += gePossibleArrangements(stringBuilder.ToString(), group);
            }
            else if (validateArrangement(arrangement, group)) { possibleArrangements++; }

            return possibleArrangements;
        }

        private static bool validateArrangement(string arrangement, List<int> group)
        {
            bool validArrangement = true;
            int currentGroupElement = 0;
            string currentGroup = "";
            for (int i = 0; i < arrangement.Length; i++)
            {
                //Placeholder --> break current loop, retrieve wether it's a possible solution to this point
                if (arrangement[i] == '?')
                {
                    break;
                }

                if (arrangement[i] == '#') { currentGroup += '#'; }
                if ((currentGroup.Length > 0) && (arrangement[i] == '.' || i == arrangement.Length - 1))
                {
                    if(currentGroupElement >= group.Count)
                    {
                        validArrangement = false;
                    }
                    else if (group[currentGroupElement] != currentGroup.Length) 
                    { 
                        validArrangement = false; 
                    }

                    currentGroup = "";
                    currentGroupElement++;
                }

                if (currentGroupElement >= group.Count && currentGroup.Length > 0) { validArrangement = false; }
                if (i == arrangement.Length - 1)
                {
                    validArrangement = validArrangement && (currentGroupElement == group.Count);
                }

                if (!validArrangement) { break; }
            }

            return validArrangement;
        }
    }
}