using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    internal class Mapping
    {
        internal Int64 DestinationStart { get; }
        internal Range Source { get; }
        internal Mapping(Int64 destinationStart, Int64 sourceRangeStart, Int64 rangeLength) {
            Source = new Range(sourceRangeStart, rangeLength);
            DestinationStart = destinationStart; 
        }

    }

    internal class Range
    {
        internal Int64 Start { get; }
        internal Int64 Length { get; }
        internal Int64 End { get { return Start + Length - 1; } }
        internal Range(Int64 start, Int64 length)
        {
            Start = start;
            Length = length;
        }
        internal Range(Int64 sourceStart, Mapping map, Int64 length)
        {
            Start = map.DestinationStart + (sourceStart - map.Source.Start);
            Length = length;
        }

    }
}
