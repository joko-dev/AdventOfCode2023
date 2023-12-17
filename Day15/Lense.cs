using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    internal class Lense
    {
        public string Label { get; }
        public int FocalLength { get; set; }
        public Lense(string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
        }
    }
}
