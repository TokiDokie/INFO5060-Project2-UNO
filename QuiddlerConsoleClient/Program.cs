using System;
using System.Collections.Generic;
using System.Linq;
using QuiddlerLibrary;

namespace QuiddlerConsoleClient
{
    /**
	 * Class Name:Program	
	 * Purpose: The client application to run Quiddler
	 * Coders: Riley and Darrell
	 * Date: 2022 - 02 - 02
    */
    class Program
    {
        static void Main(string[] args)
        {
            //Create Objects need to run program
            IDeck quiddlerDeck = new Deck();
            List<IPlayer> playerList = new List<IPlayer>();

            Console.WriteLine(quiddlerDeck.About);

            Console.WriteLine($"\nDeck initialized with the following {quiddlerDeck.CardCount} cards...");
            Console.WriteLine(quiddlerDeck.ToString());

            
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
            quiddlerDeck.CardsPerPlayer = cardsToDeal;

            Console.WriteLine("Initializing Players...\n");

            //create the players
            for (int i = 0; i < totPlayers; i++)
            {
                playerList.Add(quiddlerDeck.NewPlayer());
            }
            
            //print out how many players where dealt too
            Console.WriteLine($"Cards were dealt to {totPlayers} player(s).");
            //create the discard pile
            //by having player 1 (since there will always be one player) and have him draw and discard that card right away
            string topDrawCard = playerList[0].DrawCard();
            playerList[0].Discard(topDrawCard);
            //print out what card was drawn
            Console.WriteLine($"the top card which was '{topDrawCard}' was moved to the discard pile.");

            bool continueGame = true; // bool gate to see if players wants to continue
            while (continueGame)
            {
                string input = "";
                for (int turn = 0; turn < playerList.Count; turn++)
                {
                    //print out who turn it is and their points
                    Console.WriteLine("\n-----------");
                    Console.WriteLine($"Player {turn+1} ({playerList[turn].TotalPoints})");
                    Console.WriteLine("-----------\n");

                    //print out whats inside the deck currently
                    Console.WriteLine($"The deck now contains the following {quiddlerDeck.CardCount}...");
                    Console.WriteLine(quiddlerDeck.ToString());

                    //print out player cards
                    Console.WriteLine($"\nYour cards are [{playerList[turn].ToString()}]");

                    //while loop for yes or no that only exits if they awnser either y or n
                    input = "";
                    while (!input.Equals("y") && !input.Equals("n"))
                    {
                        Console.Write($"Do you want the top card in the discard pile which is '{quiddlerDeck.TopDiscard}'? (y/n): ");
                        input = Console.ReadLine();
                        //make input lower so it doesnt matter what case they type in
                        input = input.ToLower();
                    }

                    //if yes take top card from discard pile
                    if (input.Equals("y"))
                    {
                        playerList[turn].PickupTopDiscard();
                    }
                    else
                    {
                        //if no then draw a card from the deck and print out what left in deck
                        Console.WriteLine($"The dealer dealt '{playerList[turn].DrawCard()}' to you from the deck");
                        Console.WriteLine($"The deck contains {quiddlerDeck.CardCount} cards.");
                    }
                    //show what cards the player has
                    Console.WriteLine($"\nYour cards are [{playerList[turn].ToString()}]");

                    //loop through until input is n
                    //this will happen either the user saying no for testing a card or after the user plays cards successfully 
                    input = "";
                    while (!input.Equals("n"))
                    {
                        Console.Write("Test a word for its points value? (y/n): ");
                        input = Console.ReadLine();
                        input = input.ToLower();

                        if (input.Equals("y"))
                        {
                            Console.Write($"Enter a word using [{playerList[turn].ToString()}] leaving a space between cards: ");
                            string wordToPlay = Console.ReadLine();

                            int pointValue = playerList[turn].TestWord(wordToPlay);
                            Console.WriteLine($"The word [{wordToPlay}] is worth {pointValue} points.");
                            if (pointValue > 0)
                            {
                                
                                input = "";
                                while (!input.Equals("y") && !input.Equals("n"))
                                {
                                    Console.Write($"do you want to play the word [{wordToPlay}]? (y/n): ");
                                    input = Console.ReadLine();
                                    input = input.ToLower();
                                }
                                
                                if (input.Equals("y"))
                                {
                                    
                                    playerList[turn].PlayWord(wordToPlay);
                                    Console.WriteLine($"Your cards are [{playerList[turn].ToString()}] and you have {playerList[turn].TotalPoints} points.");
                                    input = "n";
                                }
                                else
                                {
                                    input = "";
                                }
                            }
                        }
                    }

                    //loop through until user successfully removes a card
                    bool cardRemoved = false;
                    while (!cardRemoved)
                    {
                        Console.Write("Enter a card from your hand to drop on the discard pile: ");
                        string cardToRemove = Console.ReadLine();
                        if (playerList[turn].Discard(cardToRemove))
                        {
                            cardRemoved = true;
                        }
                    }
                    Console.WriteLine($"Your cards are [{playerList[turn].ToString()}]");
                }

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

            //print out every player and their total points
            Console.WriteLine("The final scores are...");
            for (int i = 0; i < playerList.Count; i++)
            {
               Console.WriteLine($"Player {i+1}: {playerList[i].TotalPoints} points");

            }

            quiddlerDeck.Dispose();
        }

    }
}
