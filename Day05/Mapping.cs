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
        internal Int64 DestinationRangeStart { get; }
        internal Int64 SourceRangeStart { get; }
        internal Int64 RangeLength { get; }
        internal Mapping(Int64 destinationRageStart, Int64 sourceRangeStart, Int64 rangeLength) { 
            DestinationRangeStart = destinationRageStart; 
            SourceRangeStart = sourceRangeStart; 
            RangeLength = rangeLength;
        }

    }
}
