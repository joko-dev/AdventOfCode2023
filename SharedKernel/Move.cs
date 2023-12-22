using AdventOfCode2022.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.SharedKernel
{
    public class Move
    {
        public enum DirectionType { Left = 1, Right = 2, Up = 3, Down = 4 }
        public Coordinate Coordinate { get; set; }
        public DirectionType Direction { get; set; }
        public Move(Coordinate coordinate, DirectionType direction)
        {
            Coordinate = coordinate;
            Direction = direction;
        }
        public override bool Equals(object obj)
        {
            var move = obj as Move;

            if (move is null)
            {
                return false;
            }
            return (this.Coordinate.Equals(move.Coordinate) && (this.Direction == move.Direction));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coordinate, Direction);
        }

        public Move MoveToDirection()
        {
            if (Direction == DirectionType.Right)
            {
                return new Move(new Coordinate(Coordinate.X + 1, Coordinate.Y), Direction);
            }
            else if (Direction == DirectionType.Left)
            {
                return new Move(new Coordinate(Coordinate.X - 1, Coordinate.Y), Direction);
            }
            else if (Direction == DirectionType.Up)
            {
                return new Move(new Coordinate(Coordinate.X, Coordinate.Y - 1), Direction);
            }
            else if (Direction == DirectionType.Down)
            {
                return new Move(new Coordinate(Coordinate.X, Coordinate.Y + 1), Direction);
            }
            throw new NotImplementedException();
        }
        
        public Move RotateLeft()
        {
            DirectionType rotatedDirection;
            switch (Direction)
            {
                case DirectionType.Left: rotatedDirection = DirectionType.Down; break;
                case DirectionType.Right: rotatedDirection = DirectionType.Up; break;
                case DirectionType.Up: rotatedDirection = DirectionType.Left; break;
                case DirectionType.Down: rotatedDirection = DirectionType.Right; break;
                default: throw new NotImplementedException();
            }
            return new Move(Coordinate, rotatedDirection);
        }
        public Move RotateRight()
        {
            DirectionType rotatedDirection;
            switch (Direction)
            {
                case DirectionType.Left: rotatedDirection = DirectionType.Up; break;
                case DirectionType.Right: rotatedDirection = DirectionType.Down; break;
                case DirectionType.Up: rotatedDirection = DirectionType.Right; break;
                case DirectionType.Down: rotatedDirection = DirectionType.Left; break;
                default: throw new NotImplementedException();
            }
            return new Move(Coordinate, rotatedDirection);
        }
    }
}
