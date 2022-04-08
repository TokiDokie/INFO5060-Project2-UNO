using System;

namespace UNOLibrary
{
    /**
	 * Class Name:Card	
	 * Purpose: A public class to hold card information 
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    public class Card
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
            return "("+CardColour+" " +CardValue+ ") ";
        }

    } // end of class
}
