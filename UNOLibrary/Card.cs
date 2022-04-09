using System.Runtime.Serialization;

namespace UNOLibrary
{
    /*
	 * Class Name:Card	
	 * Purpose: A public class to hold card information 
	 * Coders: Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
	 * Date: 2022 - 02 - 02
    */
    [DataContract] // Attributes
    public class Card // Start of class
    {
    // Accessor Methods

        [DataMember]
        public string CardValue { get; set; }

        [DataMember]
        public string CardColour { get; set; }

    // C'tor
        public Card(string cv, string cc)
        {
            CardValue = cv;
            CardColour = cc;
        }

        /** Method Name: ToString()
        *   Purpose: Returns a formatted string which cotnains the card colour and the card value seperated by an
        *            underscore character.
        *   Accepts: Nothing.
        *   Returns: String cheese.
        *   OP: Darian Benam and Jacob McMullin */
        public override string ToString() 
        {
            var cheese = CardColour + '_' + CardValue;
            return (string)cheese;
        }
    } // end of class
}

