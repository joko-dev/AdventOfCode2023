using AdventOfCode2022.SharedKernel;
using System.Text;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Lens Library"));
            Console.WriteLine("Platform: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<int> hashes = new List<int>();
            List<string> sequences = puzzleInput.Lines[0].Split(',').ToList();
            foreach (string sequence in sequences)
            {
                hashes.Add(calculateHash(sequence));
            }
            Console.WriteLine("Sum of hashes: {0}", hashes.Sum());

            Dictionary<int, List<Lense>> boxes = new Dictionary<int, List<Lense>>();
            for (int i = 0; i <= 255; i++) { boxes.Add(i, new List<Lense>()); }
            foreach (string sequence in sequences)
            {
                boxOperation(sequence, boxes);
            }
            Console.WriteLine("focusing power: {0}", calculateFocusingPower(boxes));
        }

        private static void boxOperation(string sequence, Dictionary<int, List<Lense>> boxes)
        {
            if (sequence.Contains('-'))
            {
                string label = sequence.Substring(0, sequence.IndexOf('-'));
                for (int b = 0; b < boxes.Count; b++)
                {
                    List<Lense> box = boxes[b];
                    box.RemoveAll(b => b.Label == label);
                }
            }
            else if (sequence.Contains('='))
            {
                string[] temp = sequence.Split('=');
                string label = temp[0];
                int focalLength = int.Parse(temp[1]);
                int hash = calculateHash(label);

                List<Lense> box = boxes[hash];
                if (box.Any(l => l.Label == label))
                {
                    box.Find(l => l.Label == label).FocalLength = focalLength;
                }
                else
                {
                    box.Add(new Lense(label, focalLength));
                }
            }
            else { throw new Exception(); }
        }

        private static int calculateFocusingPower(Dictionary<int, List<Lense>> boxes)
        {
            int power = 0;
            for(int b = 0; b <= 255; b++)
            {
                List<Lense> lenses = boxes[b];
                for(int l = 0; l < lenses.Count; l++)
                {
                    power += (b + 1) * (l + 1) * lenses[l].FocalLength;
                }
            }
            return power;
        }

        private static int calculateHash(string sequence)
        {
            int hash = 0;

            for(int i = 0; i < sequence.Length; i++)
            {
                hash += (int)sequence[i];
                hash = hash * 17;
                hash = hash % 256;
            }

            return hash;
        }
    }
}
