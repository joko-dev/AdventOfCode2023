using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.SharedKernel
{
    [Serializable]
    public class InvalidCoordinateException : Exception
    {
        public InvalidCoordinateException() : base() { }
        public InvalidCoordinateException(string message) : base(message) { }
        public InvalidCoordinateException(string message, Exception inner) : base(message, inner) { }
    }

    public class Coordinate
    {

        public Coordinate(Int64 X, Int64 Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Coordinate((int x, int y) point)
        {
            this.X = point.x;
            this.Y = point.y;
        }

        public Coordinate(string coordinate)
        {
            string[] temp = coordinate.Split(',');
            X = int.Parse(temp[0]);
            Y = int.Parse(temp[1]);
        }

        public Coordinate(Coordinate coordinate) 
        { 
            this.X = coordinate.X;
            this.Y = coordinate.Y;
        }

        public void Move(Int64 xToMove, Int64 yToMove)
        {
            X += xToMove;
            Y += yToMove;
        }

        public void Move(Coordinate toMove)
        {
            Move(toMove.X, toMove.Y);
        }

        public override bool Equals(object obj)
        {
            var coordinate = obj as Coordinate;
            if(coordinate == null) 
            {
                return false;
            }
            return (this.X == coordinate.X && this.Y == coordinate.Y);
        }

        public Coordinate GetAdjacentCoordinate(Move.DirectionType direction)
        {
            int xToAdd = 0;
            int yToAdd = 0;

            switch (direction)
            {
                case SharedKernel.Move.DirectionType.Left: { xToAdd = -1; break; }
                case SharedKernel.Move.DirectionType.Right: { xToAdd = 1; break; }
                case SharedKernel.Move.DirectionType.Up: { yToAdd = -1; break; }
                case SharedKernel.Move.DirectionType.Down: { yToAdd = 1; break; }
            }

            return new Coordinate(this.X + xToAdd, this.Y + yToAdd);
        }

        public List<Coordinate> GetAdjacentCoordinates()
        {
            List<Coordinate> adjacent = new List<Coordinate>();
            adjacent.Add(new Coordinate(this.X - 1, Y - 1));
            adjacent.Add(new Coordinate(this.X, Y - 1));
            adjacent.Add(new Coordinate(this.X + 1, Y - 1));
            adjacent.Add(new Coordinate(this.X - 1, Y));
            adjacent.Add(new Coordinate(this.X + 1, Y));
            adjacent.Add(new Coordinate(this.X - 1, Y + 1));
            adjacent.Add(new Coordinate(this.X, Y + 1));
            adjacent.Add(new Coordinate(this.X + 1, Y + 1));

            return adjacent;
        }

        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }

        public static explicit operator (Int64 x, Int64 y)(Coordinate obj)
        {
            return (obj.X, obj.Y);
        }

        public static explicit operator (int x, int y)(Coordinate obj)
        {
            return ((int)obj.X, (int)obj.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}

