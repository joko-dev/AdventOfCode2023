using AdventOfCode2022.SharedKernel;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 7: Camel Cards"));
            Console.WriteLine("Cards: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Hand> hands = initializeHands(puzzleInput.Lines, false);
            Console.WriteLine("Total winnings: {0}", calculateTotalWinnings(hands));

            hands = initializeHands(puzzleInput.Lines, true);
            Console.WriteLine("Total winnings: {0}", calculateTotalWinnings(hands));
        }

        private static int calculateTotalWinnings(List<Hand> hands)
        {
            int totalWinnings = 0;

            hands.Sort();

            for (int i = 0; i < hands.Count; i++) 
            {
                totalWinnings += (i + 1) * hands[i].Bid;
            }

            return totalWinnings;
        }

        private static List<Hand> initializeHands(List<string> lines, bool withJoker)
        {
            List<Hand> result = new List<Hand>();

            foreach (string line in lines)
            {
                string[] temp = line.Split(' ');
                result.Add(new Hand(temp[0], Int32.Parse(temp[1]), withJoker));
            }

            return result;
        }
    }
}