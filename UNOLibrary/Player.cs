using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNOLibrary
{
    /**
	 * Class Name: Player	
	 * Purpose: an class to that interacts with Deck object to play the game Quiddler 
	 * Coders: Riley and Darrell
	 * Date: 2022 - 04 - 05
    */
    class Player : IPlayer
    {
        //private SpellChecker spellChecker = new SpellChecker();

        private Deck playerDeck = new Deck();


        private List<Card> playerHand = new List<Card>();

        //constructor
        public Player(Deck d)
        {
            playerDeck = d;
        }

        //Interface Implementations
        public int CardCount => playerHand.Count;


        /** Method Name: DrawCard()
        * Purpose: Draw a card from the deck and adds it too hand
        * Accepts: nothing
        * Returns: string
        * OP: Riley
        */
        //NEED TO REDUCE THE CARD SHOE FOR ACCURATE DECK NUMBERS
        public Card DrawCard()
        {

            if (playerDeck.cardShoe.Count <= 0)
            {
                throw new InvalidOperationException();
            }

            Card drawnCard = playerDeck.cardShoe[0];
            playerDeck.cardShoe.RemoveAt(0);
            playerHand.Add(drawnCard);

            return drawnCard;
        }

        /** Method Name: Discard()
        * Purpose: Discards a card from player's hand
        * Accepts: string: card
        * Returns: bool
        * OP: Riley
        */
        public bool Discard(Card card)
        {
            foreach (var cardInHand in playerHand)
            {
                if (cardInHand.Equals(card))
                {
                    //ADD:
                    //DISCARD THE CARD INTO A DISCARD PILE
                    playerDeck.discardCard = card;
                    playerHand.Remove(card);
                    return true;
                }
            }

            return false;
        }


        /** Method Name: PlayWord()
        * Purpose: Checks to see if the card can be played 
        * Accepts: string: candidate
        * Returns: bool
        * OP: Toki (Darrell)
        */
        public bool PlayWord(string candidate)
        {

            string[] splitedCardCandiadate = candidate.Split(' ');

            Card result = playerHand.Find(x => x.CardColour == splitedCardCandiadate[0]
                                             && x.CardValue == splitedCardCandiadate[1]);

        // Safe Guard - check if player HAS CARD IN HAND
            if (!playerHand.Contains(result)) return false;

            // split the incoming candiadate card (ex Green 4) and check the top discard to
            // see if it is able to be played

        // If Able to play card
            if (splitedCardCandiadate[0] == playerDeck.TopDiscard.CardColour ||
                splitedCardCandiadate[1] == playerDeck.TopDiscard.CardValue ||
                splitedCardCandiadate[0] == "Black")
            {
            // Remove card from hand
                playerHand.Remove(result);

            // SET THE result to TOP DISCARD TO UPDATE PILE
                playerDeck.discardCard = result;


                // TODO: HAVE A BOOL TO HAVE A TRACKER OF TURN DIRECTION
                //              DUE TO HAVING SWAP DIRECTION CARDS

                // Do checks to see if card value is skip or reverse order



            // Check if its Black
                if(splitedCardCandiadate[0] == "Black")
                {
                    switch(splitedCardCandiadate[1])
                    {
                        case "+2":
                            break;
                        case "+4":
                            break;
                        case "SwapColour":
                            break;
                    }
                }





                return true;
            }
            else { return false; }

        }

        /** Method Name: ToString()
        * Purpose: returns the player's hand
        * Accepts: nothing
        * Returns: string
        * OP: Riley
        */
        string IPlayer.ToString()
        {
            //make the playerHand a single string seperated by spaces
            string output;
            output = string.Join(" ", playerHand);
            return output;

        }

    }
}
