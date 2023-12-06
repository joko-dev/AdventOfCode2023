using AdventOfCode2022.SharedKernel;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 6: Wait For It"));
            Console.WriteLine("Paper: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<Race> races = createRaces(puzzleInput.Lines);
            List<Int64> numberOfWins = determineNumberOfWins(races);
            Console.WriteLine("Number of wins multiplied: {0}", numberOfWins.Aggregate((a,b) => a*b));

            races = createRacesFixKerning(puzzleInput.Lines);
            numberOfWins = determineNumberOfWins(races);
            Console.WriteLine("Number of wins multiplied: {0}", numberOfWins.Aggregate((a, b) => a * b));

        }

        private static List<Int64> determineNumberOfWins(List<Race> races)
        {
            List<Int64> result = new List<Int64>();

            foreach (Race race in races) 
            {
                int wins = 0;
                for(int timeHold = 1; timeHold < race.Time - 1; timeHold++)
                {
                    Int64 restTime = race.Time - timeHold;
                    if (restTime * timeHold > race.Distance)
                    {
                        wins++;
                    }
                }
                result.Add(wins);
            }

            return result;
        }

        private static List<Race> createRaces(List<string> lines)
        {
            List<Race> races = new List<Race>();

            string time = lines[0].Replace("Time:", "").Trim();
            while (time.Contains("  ")) time = time.Replace("  ", " ");
            string distance = lines[1].Replace("Distance:", "").Trim();
            while (distance.Contains("  ")) distance = distance.Replace("  ", " ");

            string[] times = time.Split(' ');
            string[] distances = distance.Split(' ');

            for (int i = 0; i < times.Count(); i++)
            {
                races.Add(new Race(Int32.Parse(times[i]), Int32.Parse(distances[i])));
            }

            return races;
        }
        private static List<Race> createRacesFixKerning(List<string> lines)
        {
            List<Race> races = new List<Race>();

            string time = lines[0].Replace("Time:", "").Replace(" ", "");
            string distance = lines[1].Replace("Distance:", "").Replace(" ", "");

            races.Add(new Race(Int64.Parse(time), Int64.Parse(distance)));

            return races;
        }
    }
}