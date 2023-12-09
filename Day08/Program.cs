using AdventOfCode2022.SharedKernel;
using System.Security;
using System.Xml.Linq;

namespace Day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 8: Haunted Wasteland"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            string instruction = puzzleInput.Lines[0];
            List<Node> nodes = getNodes(puzzleInput.Lines.Skip(1).ToList());
            Console.WriteLine("Steps required: {0}", getStepsRequired(instruction, nodes, "AAA", "ZZZ"));
            Console.WriteLine("Steps required ghost mode: {0}", getStepsRequiredGhost(instruction, nodes, "A", "Z"));
        }

        private static int getStepsRequiredGhost(string instruction, List<Node> nodes, string startLast, string endingLast)
        {
            // Would work, but is way too slow. Better approach needed
            int steps = 0;
            int direction = 0;
            List<Node> currentNodes = nodes.Where(n => n.Name.EndsWith(startLast)).ToList();

            while (currentNodes.Any(n =>  !n.Name.EndsWith(endingLast)))
            {
                for (int i = 0; i < currentNodes.Count; i++)
                {
                    if (instruction[direction] == 'L')
                    {
                        currentNodes[i] = nodes.Find(n => n.Name == currentNodes[i].LeftNode);
                    }
                    else
                    {
                        currentNodes[i] = nodes.Find(n => n.Name == currentNodes[i].RightNode);
                    }
                }

                steps++;
                direction++;
                if (direction >= instruction.Length) { direction = 0; }
            }

            return steps;
        }

        private static int getStepsRequired(string instruction, List<Node> nodes, string start, string target)
        {
            int steps = 0;
            int direction = 0;
            Node node = nodes.Find(n => n.Name == start);

            while (node is not null && node.Name != target)
            {
                if (instruction[direction] == 'L')
                {
                    node = nodes.Find(n => n.Name == node.LeftNode);
                }
                else
                {
                    node = nodes.Find(n => n.Name == node.RightNode);
                }

                steps++;
                direction++;
                if (direction >= instruction.Length) { direction = 0; }
            }

            return steps;
        }

        private static List<Node> getNodes(List<string> lines)
        {
            List<Node> nodes = new List<Node>();
            foreach(string line in lines)
            {
                string node = line.Substring(0, 3);
                string direction = line.Substring(line.Length - 10);
                direction = direction.Replace("(", "").Replace(")", "").Replace(",", "");
                string[] directions = direction.Split(' ');

                nodes.Add(new Node(node, directions[0], directions[1] ));
            }
            return nodes;
        }
    }
}