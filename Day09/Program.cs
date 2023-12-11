using AdventOfCode2022.SharedKernel;

namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 9: Mirage Maintenance"));
            Console.WriteLine("OASIS report: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int sumExtrapolated = 0;
            foreach(string line in puzzleInput.Lines)
            {
                List<int> history = line.Split(' ').Select(s => Int32.Parse(s)).ToList();
                extrapolateHistoryRight(history);
                sumExtrapolated += history.Last();
            }
            Console.WriteLine("Sum extrapolated values right: {0}", sumExtrapolated);

            sumExtrapolated = 0;
            foreach (string line in puzzleInput.Lines)
            {
                List<int> history = line.Split(' ').Select(s => Int32.Parse(s)).ToList();
                extrapolateHistoryLeft(history);
                sumExtrapolated += history.First();
            }
            Console.WriteLine("Sum extrapolated values left: {0}", sumExtrapolated);
        }

        private static void extrapolateHistoryRight(List<int> history)
        {
            List<int> difference = new List<int>();

            for(int i = 1; i < history.Count; i++)
            {
                difference.Add(history[i] - history[i - 1]);
            }

            if(difference.Any( d => d != 0))
            {
                extrapolateHistoryRight(difference);
            }

            history.Add(history[history.Count - 1] + difference.Last());            
        }

        private static void extrapolateHistoryLeft(List<int> history)
        {
            List<int> difference = new List<int>();

            for (int i = history.Count - 1; i > 0; i--)
            {
                difference.Insert(0, history[i] - history[i - 1]);
            }

            if (difference.Any(d => d != 0))
            {
                extrapolateHistoryLeft(difference);
            }

            history.Insert(0, history[0] - difference.First());
        }
    }
}