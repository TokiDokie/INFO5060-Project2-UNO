using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UNOLibrary
{
    /*
	 * Class Name: Player	
	 * Purpose: a class to that interacts with Deck object to play the game Uno
	 * Coders:  Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
	 * Date: 2022 - 04 - 07 
    */
    [DataContract]
    public class Player
    {
        [DataMember]
        public int ClientId { get; private set; }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public List<Card> PlayerHand { get; private set; } = new List<Card>();

        [DataMember]
        public bool CalledUno { get; set; }


        //constructor
        public Player(int clientId, string username)
        {
            ClientId = clientId;
            Username = username;
        }

        /** Method Name: DrawCard()
        * Purpose: Draw a card from the deck and adds it too hand
        * Accepts: nothing
        * Returns: string
        * OP: Riley
        */
        //NEED TO REDUCE THE CARD SHOE FOR ACCURATE DECK NUMBERS
        public Card DrawCard(ref Deck deck)
        {
            if (deck.Cards.Count <= 0)
            {
                throw new InvalidOperationException();
            }

            Card drawnCard = deck.Cards[0];
            deck.Cards.RemoveAt(0);

            PlayerHand.Add(drawnCard);

            return drawnCard;
        }


        /** Method Name: PlayCard()
         * Purpose: Checks to see if the card can be played 
         * Accepts: string: candidate
         * Returns: bool
         * OP: Darian, Jacob, Riley, Darrell
         */
        public bool PlayCard(ref Deck deck, string candidate, string colourArg) // ... string colour) // Outside this method you'd pass List<string> ColourParams from state @ index 0
        {
            // split the incoming candidate card (ex Green 4 or Black +4 Yellow) and check the top discard to
            // see if it is able to be played
            string[] splittedCardCandidate = candidate.ToLower().Split('_');

            Card result = null;

            if (splittedCardCandidate.Length > 2)
            {
                if(splittedCardCandidate.Length == 3)
                    result = PlayerHand.Find(x => x.CardColour.ToLower() == splittedCardCandidate[0]
                                            && x.CardValue.ToLower() == splittedCardCandidate[1] + '_' + splittedCardCandidate[2]);
                
                if (splittedCardCandidate.Length == 4)
                    result = PlayerHand.Find(x => x.CardColour.ToLower() == splittedCardCandidate[0]
                                            && x.CardValue.ToLower() == splittedCardCandidate[1] + '_' + splittedCardCandidate[2] + '_' + splittedCardCandidate[3]);

            }
            else
            {
                result = PlayerHand.Find(x => x.CardColour.ToLower() == splittedCardCandidate[0]
                                             && x.CardValue.ToLower() == splittedCardCandidate[1]);
            }

            // Safe Guard - check if player HAS CARD IN HAND
            if (result is null)
            {
                return false;
            }

            //check if the card is black
            if (result.CardColour == "black")
            {
                //determine if the black card is wild or wild pickup 4
                //if wild pickup 4 -> arguments = 4
                string cardModifer = splittedCardCandidate[1];

                if(cardModifer == "pickup")
                    deck.NextTurnPickup += 4;


                PlayerHand.Remove(result);
                deck.AddDiscard(result);
                
                //Handle the player's selected color value
                deck.CurrentPlayColour = colourArg;

                return true;
            }
            else //if the card has a colour other than black
            {
                bool sameCard = result.CardValue == deck.TopDiscard.CardValue;

                if (result.CardColour == deck.CurrentPlayColour || sameCard)
                {
                    if (sameCard) //if a card is played on the same one
                        deck.CurrentPlayColour = result.CardColour;

                    // Check if its a Reverse order
                    if (splittedCardCandidate[1].Equals("reverse"))
                    {
                        if (deck.NextTurnClockWise)
                        {
                            deck.NextTurnClockWise = false;
                        }
                        else { deck.NextTurnClockWise = true; }
                    }

                    // Check if its a skip 
                    if (splittedCardCandidate[1].Equals("miss"))
                    {
                        deck.NextTurnSkip = true;
                    }

                    // Check to see if its +2 
                    if (splittedCardCandidate[1].Equals("pickup") && splittedCardCandidate[2].Equals("two"))
                    {
                        deck.NextTurnPickup += 2;
                    }

                    PlayerHand.Remove(result);
                    deck.AddDiscard(result);

                    return true;
                }
            }

            return false;
        }

        /** Method Name: ToString()
        * Purpose: Returns the current player's username.
        * Accepts: nothing
        * Returns: string
        * OP: Darian and Riley
        */
        public override string ToString()
        {
            return Username;
        }
    }
}
