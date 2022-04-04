using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{
    /**
	 * Class Name:Player	
	 * Purpose: an class to that interacts with Deck object to play the game Quiddler 
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    class Player : IPlayer
    {
        //private SpellChecker spellChecker = new SpellChecker();

        private Deck playerDeck = new Deck();

        private int playerTotalPoints = 0;

        private List<string> playerHand = new List<string>();

        //constructor
        public Player(Deck d)
        {
            playerDeck = d;
        }

        //Interface Implementations
        public int CardCount => playerHand.Count;

        public int TotalPoints => playerTotalPoints;

        /** Method Name: DrawCard()
        * Purpose: Draw a card from the deck and adds it too hand
        * Accepts: nothing
        * Returns: string
        * OP: Riley
        */
        //NEED TO REDUCE THE CARD SHOE FOR ACCURATE DECK NUMBERS
        public string DrawCard()
        {
            if (playerDeck.cardsList.Count <= 0)
            {
                throw new InvalidOperationException();
            }

            //Card drawnCard = playerDeck.Draw();
            string drawnCard = playerDeck.cardsList[0];
            playerDeck.cardsList.RemoveAt(0);
            playerHand.Add(drawnCard);


            return drawnCard;
        }

        /** Method Name: Discard()
        * Purpose: Discards a card from player's hand
        * Accepts: string: card
        * Returns: bool
        * OP: Riley
        */
        public bool Discard(string card)
        {
            string upperedCard = card.ToUpper();
            foreach (var cardInHand in playerHand)
            {
                if (cardInHand.Equals(upperedCard))
                {
                    //ADD:
                    //DISCARD THE CARD INTO A DISCARD PILE
                    playerDeck.discardCard = upperedCard;
                    playerHand.Remove(upperedCard);
                    return true;
                }
            }

            return false;
        }

        /** Method Name: PickupTopDiscard()
        * Purpose: Pickup the top card on discard pile
        * Accepts: nothing
        * Returns: string
        * OP: Riley
        */
        public string PickupTopDiscard()
        {
            //will need deck to have a string disardCard
            
            string newCard = playerDeck.discardCard;
            playerHand.Add(newCard);
            playerDeck.discardCard = null;
            return newCard;
            
        }

        /** Method Name: PlayWord()
        * Purpose: play the players word
        * Accepts: string: candidate
        * Returns: int
        * OP: Riley
        */
        public int PlayWord(string candidate)
        {
            string upperCandidate = candidate.ToUpper();
            int pointValue = TestWord(upperCandidate);
            if (pointValue <= 0) return pointValue;

            playerTotalPoints += pointValue;
            string[] splitedCandidate = upperCandidate.Split(' ');

            //loop through each card in splitedCandiate
            //useing .Remove it should remove the first orrurrence of the letter
            foreach (var card in splitedCandidate)
            {
                playerHand.Remove(card);
            }
            return pointValue;
        }

        /** Method Name: TestWord()
        * Purpose: Test the player word
        * Accepts: string: candidate
        * Returns: int
        * OP: Riley
        */
        public int TestWord(string candidate)
        {

            string upperCandidate = candidate.ToUpper();
            string[] splitedCandidate = upperCandidate.Split(' ');
            if (splitedCandidate.Length >= playerHand.Count) return 0;
            foreach (string card in splitedCandidate)
            {
                if (!playerHand.Contains(card)) return 0;
            }
            string formatedCandidate = upperCandidate.Replace(" ", "");
            formatedCandidate = formatedCandidate.ToLower();
            if (!playerDeck.spellChecker.CheckSpelling(formatedCandidate))
            {
                return 0;
            }
            return GetWordValue((upperCandidate));

            
        }

        /** Method Name: GetWordValue()
        * Purpose: Get the value of the word
        * Accepts: string: candidate
        * Returns: int
        * OP: Riley
        */
        private int GetWordValue(string candidate)
        {
            string[] splitedCandidate = candidate.Split(' ');
            int totalPoints = 0;
            foreach (var letter in splitedCandidate)
            {
                Card foundCard = playerDeck.cardShoe.Find(card => card.CardValue.Equals(letter));
                //if some how a non card letter gets in
                //this might error out, more testing needed
            }

            return totalPoints;
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
