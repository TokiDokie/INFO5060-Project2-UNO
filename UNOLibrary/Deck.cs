using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace UNOLibrary
{
    /**
	 * Class Name:Deck	
	 * Purpose: a Deck class that handles player creation and deck management
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    public class Deck : IDeck
    {
        public string About => "UNO - INFO 5060 - (Group Members names go here) \n -------------------------------------------------";

        public int CardCount => cardShoe.Count();

        //needs to throw "ArgumentOutOfRangeException" if out of range of 3 - 10
        public int CardsPerPlayer { get; set; }



    // Checking order
        public bool UNOBool { get; set; }                        // Sees if a player has only card remaining in hand
        public bool NextTurnClockWise { get; set; } = true;     // If True: turn order moves (Clockwise) - turn 2 -> 3
                                                                // If False: turn order moves (Counter Clockwise) - turn 2 -> 1
        public bool NextTurnSkip { get; set; }                // Check to see if this turn gets skipped or not
        public int NextTurnPickup { get; set; }               // Before turn stats check if next player needs to pickup if so pickup until this varaible is 0
    //end of checking order

        public Card TopDiscard => discardCard;



    // Internal
        internal List<Card> cardShoe = null;  // Will be the cards remaining 
        internal Card discardCard;       // Card the player must discard eachturn

        //NEED TO CALL spellChecker.Quit() at some point

        // C'tor 
        public Deck()
        {
            cardShoe = new List<Card>();      // Creates the deck container

            loadTemplate();

            Shuffle();                    // Shuffles 


        }

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
                cardShoe = cardShoe.OrderBy(card => rng.Next()).ToList();
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

            // Colour cards GREEN BLUE YELLOW RED # 1 - 9    Skip   Reverse   +2

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

                // +2
                    for (int j = 0; j < 2; j++)
                    {
                        cardShoe.Add(new Card("+2", colour));
                    }

                }// END COLOUR for loop 

            // BLACK cards   (x2 +4)     (x2 swapColour)
                string black = "Black";

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

    }
}
