/* File Name:       Card.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:           Non-generic class that represents a Card in a Uno deck. */

using System.Runtime.Serialization;

namespace UNOLibrary
{
    [DataContract]
    public class Card
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
            return CardColour + '_' + CardValue;
        }
    } // end of class
}

