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
        public enum DirectionType { Left, Right, Up, Down }
        public Coordinate Coordinate {get;}
        public DirectionType Direction { get; set; }
        public Move(Coordinate coordinate, DirectionType direction)
        {
            Coordinate = coordinate;
            Direction = direction;
        }
        public bool Equals(Move move)
        {
            return (this.Coordinate.Equals(move.Coordinate) && (this.Direction == move.Direction));
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
    }
}
