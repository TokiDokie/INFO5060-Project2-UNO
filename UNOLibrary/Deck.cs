/* File Name:       Deck.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:           Non-generic class that represents a Uno deck, and an accessory to that deck in the form of 
 *                  a discard pile. It also contains method for shuffling and generating a new uno deck. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UNOLibrary
{
    [DataContract]
    public class Deck
    {
        private static Random _rng;

        public int CardsPerPlayer { get; set; }

        // Sees if a player has only card remaining in hand
        public bool CalledUno { get; set; }

        // If True: turn order moves (Clockwise) - turn 2 -> 3
        // If False: turn order moves (Counter Clockwise) - turn 2 -> 1
        public bool NextTurnClockWise { get; set; }

        // Check to see if this turn gets skipped or not
        public bool NextTurnSkip { get; set; }

        // Before turn stats check if next player needs to pickup if so pickup until this varaible is 0
        public int NextTurnPickup { get; set; }

        public string CurrentPlayColour { get; set; }

        // Will be the cards remaining
        public List<Card> Cards { get; set; } = null;

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

        public Deck()
        {
            if (_rng is null)
            {
                _rng = new Random();
            }

            Cards = new List<Card>();
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

            if(card.CardColour != "black")
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

        /** Method Name: Shuffle()
          * Purpose: Shuffles the deck to make each game random
          * Accepts: Nothing    
          * Returns: Nothing
          * OP: Toki
          */
        private void Shuffle()
        {
            // Randomize the cards collection
            Cards = Cards.OrderBy(card => _rng.Next()).ToList();
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
                for (int h = 0; h < 2; h++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        Cards.Add(new Card(j.ToString(), colour));
                    }
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
