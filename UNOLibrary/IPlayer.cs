using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNOLibrary
{
    /**
	 * interface Name:IPlayer	
	 * Purpose: will be how the client will interact with the player class
	 * Coders: Riley and Darrell
	 * Date: 2022 - 04 - 05
    */
    public interface IPlayer
    {
        int CardCount { get; }

        // Methods 
        Card DrawCard();
        bool Discard(Card card);

    // Play a card
        bool PlayCard(string candidate);

    // Test if it is possible to play a card
        //int TestWord(string candidate);

        string ToString();

    }
}
