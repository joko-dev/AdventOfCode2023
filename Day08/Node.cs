using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    internal class Node
    {
        public string Name { get; }
        public string LeftNode { get; }
        public string RightNode { get; }
        public Node(string name, string leftNode, string rightNode) 
        {
            Name = name;
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}
