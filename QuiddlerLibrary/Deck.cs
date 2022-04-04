using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace QuiddlerLibrary
{
    /**
	 * Class Name:Deck	
	 * Purpose: a Deck class that handles player creation and deck management
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    public class Deck : IDeck
    {
        public string About => "Quiddler - INFO 5060 - Darrell Bryan & Riley Kipp \n -------------------------------------------------";

        public int CardCount => cardsList.Count();

        //needs to throw "ArgumentOutOfRangeException" if out of range of 3 - 10
        public int CardsPerPlayer { get; set; }


        public string TopDiscard => discardCard;



    // Internal
        internal List<Card> cardShoe = null;  // Will be the cards remaining 
        internal List<string> cardsList = null; // This will be cards being drawn ( letter only )
        internal string discardCard;       // Card the player must discard eachturn

        //NEED TO CALL spellChecker.Quit() at some point
        internal Application spellChecker = new Application();

        // C'tor 
        public Deck()
        {
            cardShoe = new List<Card>();      // Creates the deck container
            cardsList = new List<string>();  // creates the card container
            loadTemplate();


            repopulate();                  // Method - repopulates the shoe of cards


            Shuffle();                    // Shuffles 


        }


        /** Method Name: repopulate()
          * Purpose: Repopulate the proper amount of cards to the shoe
          * Accepts: Nothing
          * Returns: Nothing
          * OP: Toki
          */

        private void repopulate()
        {
            try
            {
                // Clean out the shoe
                cardsList.Clear();


                foreach (Card card in cardShoe)
                {
                    // Number of cards available
                    for (int j = 0; j < card.DeckCount; j++)
                    {
                        cardsList.Add(card.Letter);
                    }
                }
                
                //Console.WriteLine("Deck Populated");
            }
            catch (Exception ex)
            {
                throw;
            }
        } // END OF repopulate() METHOD



        /** Method Name: NewPlayer()
          * Purpose: Creates new Player obj immediately pops with CardsPerPlayer
          * Accepts: Nothing
          * Returns: IPlayer reference to the player obj
          * OP: Toki
          */
        IPlayer IDeck.NewPlayer()
        {
            IPlayer player = new Player(this);
            for(int i = 0; i < CardsPerPlayer; i++) { player.DrawCard(); }

            return player;
        }


        /** Method Name: ToString()
          * Purpose: Output the remaining cards
          * Accepts: nothing
          * Returns: string
          * OP: Toki
          */
        string IDeck.ToString()
        {
            string output = "";
            for(int i = 0; i< cardShoe.Count;i++)
            {
                output+=$"{cardShoe[i].ToString(),10}";
                //i+1 so it wont do a newline on the first iteration cause the first card to be on a line by it self 
                if ((i+1) % 8 == 0)
                {
                    output += "\n";
                }
            }

            return output;
        }


        /** Method Name: Shuffle()
          * Purpose: Shuffles the deck to make each game random
          * Accepts: Nothing    
          * Returns: Nothing
          * OP: Toki
          */
        private void Shuffle()
        {
            try
            {

                // Randomize the cards collection
                Random rng = new Random();
                cardsList = cardsList.OrderBy(card => rng.Next()).ToList();
            }
            catch (Exception ex)
            {
                throw;
                throw ex;
            }
        }


        /** Method Name: loadTemplate()
          * Purpose: loads card template into the shoe
          * Accepts: Nothing    
          * Returns: Nothing
          * OP: Toki
          */
        private void loadTemplate()
        {
            try
            {
                // Clear the template
                cardShoe.Clear();

                // Populate the template
                cardShoe.Add(new Card("A", 10, 2));
                cardShoe.Add(new Card("B", 2, 8));
                cardShoe.Add(new Card("C", 2, 8));
                cardShoe.Add(new Card("D", 4, 5));
                cardShoe.Add(new Card("E", 12, 2));
                cardShoe.Add(new Card("F", 2, 6));
                cardShoe.Add(new Card("G", 4, 6));
                cardShoe.Add(new Card("H", 2, 7));
                cardShoe.Add(new Card("I", 8, 2));
                cardShoe.Add(new Card("J", 2, 13));
                cardShoe.Add(new Card("K", 2, 8));
                cardShoe.Add(new Card("L", 4, 3));
                cardShoe.Add(new Card("M", 2, 5));
                cardShoe.Add(new Card("N", 6, 5));
                cardShoe.Add(new Card("O", 8, 2));
                cardShoe.Add(new Card("P", 2, 6));
                cardShoe.Add(new Card("Q", 2, 15));
                cardShoe.Add(new Card("R", 6, 5));
                cardShoe.Add(new Card("S", 4, 3));
                cardShoe.Add(new Card("T", 6, 3));
                cardShoe.Add(new Card("U", 6, 4));
                cardShoe.Add(new Card("V", 2, 11));
                cardShoe.Add(new Card("W", 2, 10));
                cardShoe.Add(new Card("X", 2, 12));
                cardShoe.Add(new Card("Y", 4, 4));
                cardShoe.Add(new Card("Z", 2, 14));
                cardShoe.Add(new Card("CL", 2, 10));
                cardShoe.Add(new Card("ER", 2, 7));
                cardShoe.Add(new Card("IN", 2, 7));
                cardShoe.Add(new Card("QU", 2, 9));
                cardShoe.Add(new Card("TH", 2, 9));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /** Method Name: Dispose()
          * Purpose: Close the spell checker
          * Accepts: Nothing    
          * Returns: Nothing
          * OP: Toki
          */
        public void Dispose()
        {
            spellChecker.Quit();
        }
    }
}
