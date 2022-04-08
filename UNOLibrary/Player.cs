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


        /** Method Name: PlayCard()
        * Purpose: Checks to see if the card can be played 
        * Accepts: string: candidate
        * Returns: bool
        * OP: Toki (Darrell) - Riley
        */
        public bool PlayCard(string candidate)
        {
        // split the incoming candidate card (ex Green 4 or Black +4 Yellow) and check the top discard to
        // see if it is able to be played
            string[] splitedCardCandiadate = candidate.ToLower().Split(' ');

            Card result = playerHand.Find(x => x.CardColour.ToLower() == splitedCardCandiadate[0]
                                             && x.CardValue.ToLower() == splitedCardCandiadate[1]);

        // Safe Guard - check if player HAS CARD IN HAND
            if (!playerHand.Contains(result)) return false;



        // If Able to play card
            // != Black
            if ((!splitedCardCandiadate[0].Equals("black")&&
                 (splitedCardCandiadate[0].Equals(playerDeck.TopDiscard.CardColour.ToLower()) || splitedCardCandiadate[1].Equals(playerDeck.TopDiscard.CardValue.ToLower())) )
            // == Black
                || (splitedCardCandiadate[0].Equals("black") && splitedCardCandiadate.Length.Equals(3)) )
            {
            // Remove card from hand
                playerHand.Remove(result);

            // Checks to see if UNO should be called before the start of the next players turn
                if (playerHand.Count.Equals(1))
                {
                    playerDeck.UNOBool = true;
                }

            // SET THE result to TOP DISCARD TO UPDATE PILE
                playerDeck.discardCard = result;

            // Check if its a Reverse order
                if (splitedCardCandiadate[1].Equals("reverse"))
                {
                    if (playerDeck.NextTurnClockWise)
                    {
                        playerDeck.NextTurnClockWise = false;
                    }
                    else { playerDeck.NextTurnClockWise = true; }
                }


            // Check if its a skip 
                if (splitedCardCandiadate[1].Equals("skip"))
                {
                    playerDeck.NextTurnSkip = true;
                }

            // Check to see if its +2 
                if (splitedCardCandiadate[1].Equals("+2"))
                {
                    playerDeck.NextTurnPickup += 2;
                }


            // Black Card Requirements
               if (splitedCardCandiadate[0].Equals("black") && splitedCardCandiadate.Length.Equals(3) )
               {
                    // Check to see if its +4
                    if (splitedCardCandiadate[1].Equals("+4")) 
                    {
                        playerDeck.NextTurnPickup += 4; 
                    }
                    switch (splitedCardCandiadate[2] )
                    {
                        case "green": playerDeck.TopDiscard.CardColour = "Green"; break;
                        case "blue": playerDeck.TopDiscard.CardColour = "Blue"; break;
                        case "yellow": playerDeck.TopDiscard.CardColour = "Yellow"; break;
                        case "red": playerDeck.TopDiscard.CardColour = "Red"; break;
                    }
               }
            // END Black Card Requirements

            
              
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
