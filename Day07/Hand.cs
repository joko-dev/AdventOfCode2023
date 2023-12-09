using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    internal class Hand : IComparable<Hand>
    {
        public int Bid { get; }
        public string Cards { get; }
        public int Strength { get { return calculateStrength(); } }

        private bool WithJoker { get;  }

        public Hand(string cards, int bid, bool withJoker)
        {
            Bid = bid;
            Cards = cards;
            WithJoker = withJoker;
            if (Cards.Length != 5) { throw new ArgumentException(); }
        }

        public int calculateStrength()
        {
            if (WithJoker && Cards.Contains( 'J')) 
            { 
                List<int> strengths = new List<int>();
                foreach(char card in Cards.Where( c => c != 'J').ToList())
                {
                    Hand hand = new Hand(Cards.Replace('J', card), this.Bid, false);
                    strengths.Add(hand.Strength);
                }
                if(Cards == "JJJJJ")
                {
                    strengths.Add( new Hand("AAAAA", this.Bid, false).Strength );
                }
                return strengths.Max();
            }
            else
            {
                int strength = 0;
                Dictionary<char, int> cards = new Dictionary<char, int>();
                foreach (char c in Cards)
                {
                    if (!cards.ContainsKey(c)) { cards.Add(c, 0); }
                    cards[c]++;
                }
                // High card
                if (cards.Count == 5) { strength = 1; }
                // One pair
                else if (cards.Count == 4) { strength = 2; }
                else if (cards.Count == 3)
                {
                    // Two pair
                    if (cards.Max(c => c.Value) == 2) { strength = 3; }
                    // Three of a kind
                    if (cards.Max(c => c.Value) == 3) { strength = 4; }
                }
                else if (cards.Count == 2)
                {
                    // Full house 
                    if (cards.Max(c => c.Value) == 3) { strength = 5; }
                    // Four of a kind
                    if (cards.Max(c => c.Value) == 4) { strength = 6; }
                }
                // Five of a kind
                else if (cards.Count == 1) { strength = 7; }

                return strength;
            }
        }

        public int CompareTo(Hand? other)
        {
            if (this == other) return 0;
            else if (this.Strength < other.Strength) { return -1; }
            else if (this.Strength > other.Strength) { return 1; }
            else if (this.Strength == other.Strength) 
            { 
                for (int i = 0; i < Cards.Length; i++) 
                {
                    if (getCardValue(this.Cards[i]) < getCardValue(other.Cards[i])){ return -1; }
                    else if (getCardValue(this.Cards[i]) > getCardValue(other.Cards[i])) { return 1; }
                }
            }
            throw new ArgumentException();
        }

        private int getCardValue(char card)
        {
            if (WithJoker && card == 'J') {  return 0; }
            else
            {
                switch (card)
                {
                    case '2': return 1;
                    case '3': return 2;
                    case '4': return 3;
                    case '5': return 4;
                    case '6': return 5;
                    case '7': return 6;
                    case '8': return 7;
                    case '9': return 8;
                    case 'T': return 9;
                    case 'J': return 10;
                    case 'Q': return 11;
                    case 'K': return 12;
                    case 'A': return 13;
                }
            }
            
            throw new ArgumentException();
        }
    }
}


