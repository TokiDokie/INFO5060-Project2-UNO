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
                    for (int j = 0; j < 1; j++)
                    {
                        cardsList.Add(card.CardValue);
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
                if ((i+1) % 11 == 0)
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

            // Colour cards GREEN BLUE YELLOW RED # 1 - 9    Skip   Reverse 

            // For each colour
                for (int i = 0; i < 4; i++)
                {
                    string colour = "";
                    int colourNum = i;
                    
                    switch(colourNum){
                        case 0: colour = "Green"; break;
                        case 1: colour = "Blue";  break;
                        case 2: colour = "Yellow"; break;
                        case 3: colour = "Red"; break;
                    }

                // 1-9
                    for (int j = 0; j < 9; j++)
                    {
                        cardShoe.Add(new Card((j+1).ToString(), colour));
                    }

                // Skip
                    cardShoe.Add(new Card("Skip", colour));

                // Reverse
                    cardShoe.Add(new Card("Reverse", colour));

                }// END COLOUR for loop 

            // BLACK cards (x4 +2)   (x2 +4)     (x2 swapColour)
                string black = "Black";

            // Pick up 2
                for (int i = 0; i < 4; i++)
                {
                    cardShoe.Add(new Card("+2", black));
                }

            // Pick up 4
                for (int i = 0; i < 2; i++)
                {
                    cardShoe.Add(new Card("+4", black));
                }

            // Swap Colour
                for (int i = 0; i < 2; i++)
                {
                    cardShoe.Add(new Card("SwapColour", black));
                }

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
