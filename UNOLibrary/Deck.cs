using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UNOLibrary
{
    /**
	 * Class Name:Deck	
	 * Purpose: a Deck class that handles player creation and deck management
	 * Coders: Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
	 * Date: 2022 - 04 - 07 
    */
    [DataContract]
    public class Deck
    {
        public int CardsPerPlayer { get; set; }

    // Checking order
        public bool CalledUno { get; set; }           // Sees if a player has only card remaining in hand
        public bool NextTurnClockWise { get; set; }   // If True: turn order moves (Clockwise) - turn 2 -> 3
                                                      // If False: turn order moves (Counter Clockwise) - turn 2 -> 1
        public bool NextTurnSkip { get; set; }        // Check to see if this turn gets skipped or not
        public int NextTurnPickup { get; set; }       // Before turn stats check if next player needs to pickup if so pickup until this varaible is 0
        public string CurrentPlayColour { get; set; }    
    // End of checking order

        public List<Card> Cards { get; set; } = null; // Will be the cards remaining

        [DataMember]
        public Stack<Card> DiscardPile = null;

        public Card TopDiscard
        {
            get
            {
                if (DiscardPile.Count == 0)
                {
                    throw new Exception("Player's hand is empty!");
                }

                return DiscardPile.Peek();
            }
        }

        // C'tor 
        public Deck()
        {
            Cards = new List<Card>();               // Creates the deck container
            DiscardPile = new Stack<Card>();
            NextTurnClockWise = true;

            LoadTemplate();
            Shuffle();
        }

        public void RecycleCard(Card card)
        {
            Cards.Add(card);
        }

        public void AddDiscard(Card card)
        {
            DiscardPile.Push(card);
            CurrentPlayColour = card.CardColour;
        }

        public Card DrawCard()
        {
            if (Cards.Count == 0)
            {
                List<Card> newDeck = new List<Card>();

                while (DiscardPile.Count > 1)
                {
                    newDeck.Add(DiscardPile.Pop()); 
                }

                Cards = newDeck;
                Shuffle();
            }

            Card topCard = Cards.Last();
            Cards.RemoveAt(Cards.Count - 1);

            return topCard;
        }

        /** Method Name: ToString()
          * Purpose: Output the remaining cards
          * Accepts: nothing
          * Returns: string
          * OP: Toki
          */
        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < Cards.Count; i++)
            {
                output += $"{Cards[i].ToString(),10}";
                //i+1 so it wont do a newline on the first iteration cause the first card to be on a line by it self 
                if ((i + 1) % 11 == 0)
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
            // Randomize the cards collection
            Random rng = new Random();
            Cards = Cards.OrderBy(card => rng.Next()).ToList();
        }

        /** Method Name: LoadTemplate()
          * Purpose: loads card template into the shoe
          * Accepts: Nothing    
          * Returns: Nothing
          * OP: Toki
          */
        private void LoadTemplate()
        {
        // Clear the template
            Cards.Clear();

        // Populate the template

        // Colour cards GREEN BLUE YELLOW RED # 1 - 9    Skip   Reverse   +2

        // For each colour
            for (int i = 0; i < 4; i++)
            {
                string colour = "";
                int colourNum = i;
                    
                switch (colourNum)
                {
                    case 0: colour = "green"; break;
                    case 1: colour = "blue";  break;
                    case 2: colour = "yellow"; break;
                    case 3: colour = "red"; break;
                }

            // 1-9
                for (int j = 0; j < 9; j++)
                {
                    Cards.Add(new Card((j + 1).ToString(), colour));
                }

            // Skip
                Cards.Add(new Card("miss", colour));

            // Reverse
                Cards.Add(new Card("reverse", colour));

            // +2
                for (int j = 0; j < 2; j++)
                {
                    Cards.Add(new Card("pickup_two", colour));
                }

            }// END COLOUR for loop 

        // BLACK cards   (x2 +4)     (x2 swapColour)
            string black = "black";

        // Pick up 4
            for (int i = 0; i < 2; i++)
            {
                Cards.Add(new Card("pickup_four_wild", black));
            }

        // Pick up Swap Colour card
            for (int i = 0; i < 2; i++)
            {
                Cards.Add(new Card("wild", black));
            }
        }
    }
}
