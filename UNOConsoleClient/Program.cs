using System;
using System.Collections.Generic;
using System.Linq;
using UNOLibrary;

namespace UNOConsoleClient
{
    /**
	 * Class Name:Program	
	 * Purpose: The client application to run UNO game
	 * Coders: Riley - Toki (Darrell)
	 * Date: 2022 - 04 - 05
    */
    class Program
    {
        static void Main(string[] args)
        {
            //Create Objects need to run program
            IDeck unoDeck = new Deck();
            List<IPlayer> playerList = new List<IPlayer>();

            Console.WriteLine(unoDeck.About);

        // REMOVE BEFORE RELEASE
        // This is just to see all cards added to list
            Console.WriteLine($"\nDeck initialized with the following {unoDeck.CardCount} cards...");
            Console.WriteLine(unoDeck.ToString());

            
            int totPlayers = 0; //variable to hold user input total players
            bool noLetters = true;//bool gate to pass user input too totPlayers
            //this could be a try catch maybe :thinking: infact most of these input validations
            //loop through until user can input a number between 1 and 8
            while (totPlayers < 1 || totPlayers > 8)
            {
                Console.Write("\nHow many players are there? (1-8) : ");
                string input = Console.ReadLine();
                //if a non digit is found flip the gate
                foreach (char c in input)
                {
                    if (!char.IsDigit(c))
                        noLetters = false;
                }

                if (noLetters)//if input is 100% numbers then pass it too totPlayers
                    totPlayers = int.Parse(input);
                else//if a letter is found reset and try again
                    noLetters = true;
            }

          

            int cardsToDeal = 0; 
            noLetters = true; //reset bool gate to true
            //loop through until user can input cards to deal number between 3 and 10
            while (cardsToDeal < 3 || cardsToDeal > 10)
            {
                Console.Write("How many cards will be dealt to each player? (3-10) : ");
                string input = Console.ReadLine();
                foreach (char c in input)
                {
                    if (!char.IsDigit(c))
                        noLetters = false;
                }

                if (noLetters)
                    cardsToDeal = int.Parse(input);
                else
                    noLetters = true;
            }
            //set the deck cards per player too the user input cards to deal
            unoDeck.CardsPerPlayer = cardsToDeal;

            Console.WriteLine("Initializing Players...\n");

            //create the players
            for (int i = 0; i < totPlayers; i++)
            {
                playerList.Add(unoDeck.NewPlayer());
            }
            
            //print out how many players where dealt too
            Console.WriteLine($"Cards were dealt to {totPlayers} player(s).");
            //create the discard pile
            //by having player 1 (since there will always be one player) and have him draw and discard that card right away
            Card topDrawCard = playerList[0].DrawCard();
            playerList[0].Discard(topDrawCard);
            //print out what card was drawn
            Console.WriteLine($"the top card which was a '{unoDeck.TopDiscard.CardColour} {unoDeck.TopDiscard.CardValue}' was moved to the discard pile.");

            bool continueGame = true; // bool gate to see if players wants to continue
            while (continueGame)
            {
                string input = "";
                for (int turn = 0; turn < playerList.Count; turn++)
                {
                    //print out who turn it is and their points
                    Console.WriteLine("\n-----------");
                    Console.WriteLine($"Player {turn + 1}, Your move");
                    Console.WriteLine("-----------\n");


              /**
	            * Purpose: The area where the game is played
	            * Coders: Toki (Darrell)
	            * Date: 2022 - 04 - 05
                */
            #region Uno 

                    input = "";
                    while (!input.Equals("p") && (!input.Equals("d")) )
                    {
                        //print out whats inside the deck currently
                        Console.WriteLine($"The deck now contains {unoDeck.CardCount} cards left...");
                        Console.WriteLine($"{unoDeck.CardCount} in deck...");
                        for (int i = 0; i < totPlayers; i++)
                        {
                            Console.WriteLine($"Player {i+1} has {playerList[i].CardCount} cards left.");
                        }

                        Console.WriteLine("\n");

                        Console.WriteLine($"The top card is a '{unoDeck.TopDiscard.CardColour} {unoDeck.TopDiscard.CardValue}'.");

                        //print out player cards
                        Console.WriteLine($"\nYour cards are [{playerList[turn].ToString()}]");

                        //loop through until input is not P or D 
                        //this will happen after the user plays cards successfully 
                    
                        Console.Write("(P)lay a card or (D)raw a card - (P/D): ");
                        input = Console.ReadLine();
                        input = input.ToLower();

                        if (input.Equals("p"))
                        {
                            bool correctCardBool = false;
                            while (!correctCardBool)
                            {
                                Console.Write($"Enter a card using [{playerList[turn].ToString()}] leaving a space between values. Ex: Green 4 ");
                                string cardToPlay = Console.ReadLine();

                            // Check if card can be played

                                if(playerList[turn].PlayWord(cardToPlay))
                                {
                                    Console.WriteLine("Card played");
                                    correctCardBool = true;
                                }
                                else
                                {
                                    Console.WriteLine($"The top card is a '{unoDeck.TopDiscard.CardColour} {unoDeck.TopDiscard.CardValue}'.");

                                    //print out player cards
                                    Console.WriteLine($"\nYour cards are [{playerList[turn].ToString()}]");
                                }
                            }


                        }// END input.Equals("p")
                        else if(input.Equals("d"))
                        {
                            playerList[turn].DrawCard();
                        }
                        else {
                            input = "";
                        }
                    }//END LOOP P D

                    
                    Console.WriteLine($"Your cards are [{playerList[turn].ToString()}]");
                }// END while 

            #endregion

                input = "";
                //while loop to see if players will want to end the game
                while (!input.Equals("y") && !input.Equals("n"))
                {
                    bool endGame = false;
                    foreach (IPlayer player in playerList)
                    {
                        if (player.CardCount== 0)
                        {
                            endGame = true;
                        }

                    }
                    if (!endGame)
                    {
                        Console.Write("Would you like each player take another turn? (y/n): ");
                        input = Console.ReadLine();
                        input = input.ToLower();
                    }
                    else
                    {
                        Console.Write("A player is out of cards now ");
                        input = "n";
                    }
                }
                if (input.Equals("n"))
                {
                    continueGame = false;
                }
            }

            Console.WriteLine("Retiring the game.");

            // TODO: Check amount of cards each player has and declare a winner
    

            unoDeck.Dispose();
        }

    }
}
