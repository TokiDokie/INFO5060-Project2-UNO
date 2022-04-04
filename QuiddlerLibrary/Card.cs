using System;

namespace QuiddlerLibrary
{
    /**
	 * Class Name:Card	
	 * Purpose: an internal class to hold card information 
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    internal class Card
    {
        private string letter;
        private int deckCount;
        private int pointValue;

    // Accessor Methods
        public string Letter { get { return letter; } set { letter = value; } }             
        public int DeckCount { get { return deckCount; } set { deckCount = value; } }
        public int PointValue { get { return pointValue; } set { pointValue = value; } }

    // C'tor
        public Card(string l, int c, int v)
        {
            this.letter = l;
            this.deckCount = c;
            this.pointValue = v;
        }

        public void ReducePoint()
        {
            this.DeckCount--;
        }

        public override string ToString() 
        {
            return letter+ "(" + DeckCount + ") ";
        }




        //public override string ToString()
        //{
        //    return Rank.ToString() + " of " + Suit.ToString();
        //}

    } // end of class
}
