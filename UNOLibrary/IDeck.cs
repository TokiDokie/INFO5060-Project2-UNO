using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNOLibrary
{
    /**
	 * interface Name: IDeck	
	 * Purpose: an interface for deck and will be how client will interact with the Quiddler game
	 * Coders: Riley and Darrell
	 * Date: 2022 - 04 - 05
    */
    public interface IDeck 
    {
        string About { get; }           // Name of project and Developers
        int CardCount { get; }          // Amount of cards left undealt 
        int CardsPerPlayer { get; set; } // 3-10 inclusive. How many cards per player
        bool UNOBool { get; set; }      // Does a player only have ONE card in hand?
        bool NextTurnClockWise { get; set; } // Which direction is the game going?
        bool NextTurnSkip { get; set; }     // Should the next players turn be SKIPED?
        int NextTurnPickup { get; set; }    // Should the next player pick up any cards?

        Card TopDiscard { get; }      // Keeps track of top discarded card
                                        // If uninitialized when read initialize it 

        IPlayer NewPlayer();            // Creates new Player obj
                                        // immediately pops with CardsPerPlayer
                                        // returns IPlayer reference to the player obj
        string ToString();                // Prints out templated alphabetically with letter value
    }
}
