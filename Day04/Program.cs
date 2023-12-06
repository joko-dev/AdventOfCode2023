using AdventOfCode2022.SharedKernel;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 4: Scratchcards"));
            Console.WriteLine("Scratchcards: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<int> pointsScratchcard = getPointsScratchcard(puzzleInput.Lines);
            Console.WriteLine("Sum of Part numbers: {0}", pointsScratchcard.Sum());

            int sumScratcards = getSumScratchcards(puzzleInput.Lines);
            Console.WriteLine("Sum of scratchcards: {0}", sumScratcards);
        }

        private static int getSumScratchcards(List<string> lines)
        {
            Dictionary<int, int> cards = new Dictionary<int, int>();
            for (int i = 1; i <= lines.Count; i++)
            {
                cards.Add(i, 1);
            }

            for (int scratchcard = 1; scratchcard <= lines.Count; scratchcard++)
            {
                List<int> intersect = getMatchingNumbersFromScratchcard(lines[scratchcard-1]);

                for(int i = 1; i <= intersect.Count; i++)
                {
                    if (cards.ContainsKey(scratchcard + i))
                    {
                        cards[scratchcard + i] += cards[scratchcard];
                    }
                }
                
            }
            return cards.Select(c => c.Value).Sum();
        }

        private static List<int> getMatchingNumbersFromScratchcard(string line)
        {
            string[] temp = line.Remove(0, line.IndexOf(':') + 2).Replace("  ", " ").Split('|');

            List<int> winningNumbers = temp[0].Trim().Split(' ').Select(s => Int32.Parse(s)).ToList();
            List<int> ownNumbers = temp[1].Trim().Split(' ').Select(s => Int32.Parse(s)).ToList();

            return winningNumbers.Intersect(ownNumbers).ToList();
        }

        private static List<int> getPointsScratchcard(List<string> lines)
        {
            List<int> points = new List<int>();

            foreach (string line in lines)
            {
                List<int> intersect = getMatchingNumbersFromScratchcard(line);

                if(intersect.Count > 0)
                {
                    points.Add((int)Math.Pow(2, intersect.Count - 1));
                }
                else
                {
                    points.Add(0);
                }

            }

            return points;
        }
    }
}