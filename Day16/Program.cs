using AdventOfCode2022.SharedKernel;

namespace Day16
{
    internal class Program
    {
        public static int maxTiles = 10;
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 16: The Floor Will Be Lava"));
            Console.WriteLine("Contraption layout: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            char[,] layout = PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null);
            Console.WriteLine("energized tiles: {0}", getEnergizedTiles(layout, new Move(new Coordinate(-1, 0), Move.DirectionType.Right)));

            List<int> energizedTiles = new List<int>();
            {
                for(int x = 0; x < layout.GetLength(0); x++) 
                {
                    energizedTiles.Add(getEnergizedTiles(layout, new Move(new Coordinate(x, -1), Move.DirectionType.Down)));
                    energizedTiles.Add(getEnergizedTiles(layout, new Move(new Coordinate(x, layout.GetLength(1)), Move.DirectionType.Up)));
                }
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    energizedTiles.Add(getEnergizedTiles(layout, new Move(new Coordinate(-1, y), Move.DirectionType.Right)));
                    energizedTiles.Add(getEnergizedTiles(layout, new Move(new Coordinate(layout.GetLength(0), y), Move.DirectionType.Left)));
                }
            }
            Console.WriteLine("largest energized tiles: {0}", energizedTiles.Max());
        }

        private static int getEnergizedTiles(char[,] layout, Move start )
        {
            List<LinkedList<Move>> beams = new List<LinkedList<Move>>();
            startNewBeam( layout, start, beams );

            List<Coordinate> energizedTiles = new List<Coordinate>();
            foreach(LinkedList<Move> beam in beams)
            {
                foreach(Move move in beam)
                {
                    if(!energizedTiles.Any(t => t.Equals(move.Coordinate)))
                    {
                        energizedTiles.Add(move.Coordinate);
                    }
                }
            }

            //char[,] temp = (char[,])layout.Clone();
            //foreach(Coordinate tile in energizedTiles)
            //{
            //    temp[tile.X, tile.Y] = '#';
            //}
            //foreach (string line in PuzzleOutputFormatter.outputMap(temp))
            //{
            //    Console.WriteLine(line);
            //}

            return energizedTiles.Count;
        }

        private static void startNewBeam(char[,] layout, Move start, List<LinkedList<Move>> beams)
        {
            LinkedList<Move> currentBeam = new LinkedList<Move>();
            Move move = start;
            beams.Add(currentBeam);
            // Small hack for ignoring starting point;
            if(start.Coordinate.X >= 0 && start.Coordinate.X < layout.GetLength(0)
                && start.Coordinate.Y >= 0 && start.Coordinate.Y < layout.GetLength(1))
            {
                currentBeam.AddLast(move);
            }

            while (move != null)
            {
                getNextMoves(layout, move, currentBeam, out Move nextMove1, out Move nextMove2);
                move = null;

                if (nextMove1 != null && !beams.Any(b => b.Any(m => m.Equals(nextMove1))))
                {
                    currentBeam.AddLast(nextMove1);
                    move = nextMove1;
                }
                if (nextMove2 != null && !beams.Any(b => b.Any(m => m.Equals(nextMove2))))
                {
                    startNewBeam( layout, nextMove2, beams );
                }
            }
        }

        private static void getNextMoves(char[,] layout, Move move, LinkedList<Move> currentBeam, out Move nextMove1, out Move nextMove2)
        {
            // Edge reached --> beam end, early return
            if ((move.Direction == Move.DirectionType.Left && move.Coordinate.X == 0)
                || (move.Direction == Move.DirectionType.Right && move.Coordinate.X == layout.GetLength(0) - 1)
                || (move.Direction == Move.DirectionType.Up && move.Coordinate.Y == 0)
                || (move.Direction == Move.DirectionType.Down && move.Coordinate.Y == layout.GetLength(1) - 1))
            {
                nextMove1 = null;
                nextMove2 = null;
                return;
            }

            nextMove1 = move.MoveToDirection();
            nextMove2 = null;
            char tile = layout[nextMove1.Coordinate.X, nextMove1.Coordinate.Y];
            if (    (tile == '.') 
                    || (tile == '-' && (nextMove1.Direction == Move.DirectionType.Left || nextMove1.Direction == Move.DirectionType.Right))
                    || (tile == '|' && (nextMove1.Direction == Move.DirectionType.Up || nextMove1.Direction == Move.DirectionType.Down))
                )
            {
                return;
            }
            else if (tile == '/')
            {
                if(nextMove1.Direction == Move.DirectionType.Right) { nextMove1.Direction = Move.DirectionType.Up; }
                else if (nextMove1.Direction == Move.DirectionType.Left) { nextMove1.Direction = Move.DirectionType.Down; }
                else if (nextMove1.Direction == Move.DirectionType.Down) { nextMove1.Direction = Move.DirectionType.Left; }
                else if (nextMove1.Direction == Move.DirectionType.Up) { nextMove1.Direction = Move.DirectionType.Right; }
                else { throw new ArgumentException(); }

                return;
            }
            else if (tile == '\\')
            {
                if(nextMove1.Direction == Move.DirectionType.Right) { nextMove1.Direction = Move.DirectionType.Down; }
                else if (nextMove1.Direction == Move.DirectionType.Left) { nextMove1.Direction = Move.DirectionType.Up; }
                else if (nextMove1.Direction == Move.DirectionType.Down) { nextMove1.Direction = Move.DirectionType.Right; }
                else if (nextMove1.Direction == Move.DirectionType.Up) { nextMove1.Direction = Move.DirectionType.Left; }
                else { throw new ArgumentException(); }

                return;
            }
            else if (tile == '-')
            {
                if (nextMove1.Direction == Move.DirectionType.Up || nextMove1.Direction == Move.DirectionType.Down) 
                { 
                    nextMove1.Direction = Move.DirectionType.Left;
                    nextMove2 = new Move(new Coordinate(nextMove1.Coordinate), Move.DirectionType.Right);
                }
                else { throw new ArgumentException(); }

                return;
            }
            else if (tile == '|')
            {
                if (nextMove1.Direction == Move.DirectionType.Left || nextMove1.Direction == Move.DirectionType.Right)
                {
                    nextMove1.Direction = Move.DirectionType.Up;
                    nextMove2 = new Move(new Coordinate(nextMove1.Coordinate), Move.DirectionType.Down);
                }
                else { throw new ArgumentException(); }

                return;
            }

            throw new ArgumentException();
        }
    }
}
