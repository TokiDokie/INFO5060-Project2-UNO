using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{
    /**
	 * interface Name:IPlayer	
	 * Purpose: will be how the client will interact with the player class
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    public interface IPlayer
    {
        int CardCount { get; }
        int TotalPoints { get; }

        // Methods 
        string DrawCard();
        bool Discard(string card);
        string PickupTopDiscard();
        int PlayWord(string candidate);
        int TestWord(string candidate);

        string ToString();







    }
}
