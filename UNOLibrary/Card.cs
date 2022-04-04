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
        private string cardValue;

        private string cardColour;

    // Accessor Methods
        public string CardValue { get { return cardValue; } set { cardValue = value; } }             
 
        public string CardColour { get { return cardColour; } set { cardColour = value; } }

    // C'tor
        public Card(string cv, string cc)
        {
            this.cardValue = cv;

            this.cardColour = cc;
        }



        public override string ToString() 
        {
            return CardValue + "(" + CardColour + ") ";
        }

    } // end of class
}
