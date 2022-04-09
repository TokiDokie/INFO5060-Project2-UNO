/* File Name:       UnoGame.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UNOLibrary.DataStructures;
using UNOLibrary.Networking.Entities;

namespace UNOLibrary.Networking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UnoGame : IUnoGame
    {
        public const int MinPlayers = 2;
        public const int MaxPlayers = 8;

        private static int _nextClientId;

        // NOTE: int = Client ID, ICallback = that client's ICallback instance
        private Dictionary<int, ICallback> _callbacks;
        private GameState _gameState;
        private CircularLinkedList<Player> _playerListCircular;
        private Random _random;

        private bool _gameStarted;

        public UnoGame()
        {
            _nextClientId = -1;
            _callbacks = new Dictionary<int, ICallback>();
            _playerListCircular = new CircularLinkedList<Player>();
            _random = new Random();
            _gameStarted = false;
        }

        public int TryJoinGame(string username)
        {
            ICallback invokerCb = OperationContext.Current.GetCallbackChannel<ICallback>();

            // Don't allow users to join an ongoing game
            if (_gameStarted)
            {
                invokerCb.DisplayErrorMessage("Sorry, you can't join the lobby because a game has already been started!");
                return -1;
            }

            if (_callbacks.ContainsValue(invokerCb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(invokerCb);
                return _callbacks.Keys.ElementAt(callbackIndex);
            }

            _nextClientId++;

            _callbacks.Add(_nextClientId, invokerCb);

            Player joinedPlayer = new Player(_nextClientId, username);

            //_playersList.Add(joinedPlayer);
            _playerListCircular.AddToEnd(joinedPlayer);

            Console.WriteLine($"{username} joined - id: {_nextClientId}");

            return _nextClientId;
        }

        public void LeaveGame()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(cb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(cb);
                int clientId = _callbacks.ElementAt(callbackIndex).Key;
                Player playerToRemove = GetPlayerByClientId(clientId);

                _callbacks.Remove(clientId);

                if (!(playerToRemove is null))
                {
                    _playerListCircular.RemoveFirst(playerToRemove);
                }

                UpdateWaitingList();
            }
        }

        public void UpdateWaitingList()
        {
            foreach (ICallback cb in _callbacks.Values)
            {
                cb.UpdateWaitingList(new PlayerList(_playerListCircular.ToList(), MinPlayers, MaxPlayers));
            }
        }

        public void TryStartGame()
        {
            ICallback invokerCb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(invokerCb))
            {
                if (_playerListCircular.Length >= MinPlayers)
                {
                    foreach (ICallback cb in _callbacks.Values)
                    {
                        cb.StartNewGame();
                    }

                    StartGame();
                }
                else
                {
                    invokerCb.DisplayErrorMessage("Minimum players requirements has not been met yet!");
                }
            }
        }

        private void StartGame()
        {
            _gameStarted = true;

            int randomPlayerIndex = _random.Next(0, _playerListCircular.Length);
            Player initialTurnPlayer = _playerListCircular[randomPlayerIndex];

            // Initialize a new game state
            _gameState = new GameState(initialTurnPlayer.ClientId, new Deck(), _playerListCircular.ToList());

            // Populate players card hand. The reason why this is two loops instead of one is because in a real game of UNO, the
            // dealer would deal one card at a time to a player (i.e., Player 1 gets a card, then Player 2 gets a card, etc.).
            for (int i = 0; i < 7; i++)
            {
                foreach (Player player in _playerListCircular)
                {
                    player.DrawCard(ref _gameState.Deck);
                }
            }

            Card cardToDraw = _gameState.Deck.DrawCard();

            // Recycle all black cards as according to UNO rules, they must be added to the back of the deck
            while (cardToDraw.CardColour == "black")
            {
                _gameState.Deck.RecycleCard(cardToDraw);
            }

            // Add one card to the discard pile (to prevent null exception and be able to play the game)
            // NOTE: The card being added to the discard should never be black
            _gameState.Deck.AddDiscard(cardToDraw);

            _gameState.AddLogMessage("The UNO game has started!");

            // Send the state to all players
            UpdateAllPlayersGameState();
        }

        private void UpdateAllPlayersGameState()
        {
            foreach (ICallback cb in _callbacks.Values)
            {
                cb.UpdateGameState(_gameState);
            }
        }

        public bool TryPlayCard(string card, string changeCardColour)
        {
            //find the player
            ICallback invokerCb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(invokerCb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(invokerCb);
                int clientId = _callbacks.ElementAt(callbackIndex).Key;

                Player invokerPlayer = GetPlayerByClientId(clientId);

                //see if they have the card to play
                Card triedCard = invokerPlayer.PlayerHand.FirstOrDefault(c => card == c.ToString());

                bool validPlay = invokerPlayer.PlayCard(ref _gameState.Deck, triedCard.ToString(), changeCardColour);
               
                //update the game state AFTER the next turn has been figured out
                if (validPlay)
                {
                    //check if the game has been won
                    if (invokerPlayer.PlayerHand.Count == 0)
                    {
                        _gameState.AddLogMessage($"The UNO game been won by player {invokerPlayer.Username}!");

                        return validPlay;
                    }
                    else
                    {
                        //if the player forgot to call uno
                        if (invokerPlayer.PlayerHand.Count == 1 && !invokerPlayer.CalledUno)
                        {
                            for (int i = 0; i < 2; i++)
                                invokerPlayer.DrawCard(ref _gameState.Deck);
                        }

                        invokerPlayer.CalledUno = false; //reset for next attempt

                        //Use deck flags to figure out values for next turn
                        DetermineNextTurn();

                        UpdateAllPlayersGameState();
                    }
                    
                }

                return validPlay;
            }

            return false;
        }

        public void DrawCard(int numCards) //will be called with 1 by default?
        {
            //get player
            ICallback invokerCb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(invokerCb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(invokerCb);
                int clientId = _callbacks.ElementAt(callbackIndex).Key;

                Player invokerPlayer = GetPlayerByClientId(clientId);

                //add the number of cards specified to their hand

                for (int i = 0; i < numCards; i++)
                {
                    invokerPlayer.PlayerHand.Add(_gameState.Deck.DrawCard());
                }
            }
        }

        private Player GetPlayerByClientId(int clientId)
        {
            List<Player> playersList = _playerListCircular.ToList();
            return playersList.FirstOrDefault(player => player.ClientId == clientId);
        }

        public void DetermineNextTurn()
        {
            //see who is next to play (what direction are we going, has a miss been played?)

            int nextPlayerIndex;
            bool skip = _gameState.Deck.NextTurnSkip;

            //use a circle
            if (_gameState.Deck.NextTurnClockWise)
            {
                nextPlayerIndex = _playerListCircular.MoveNext();

                if(skip)
                    nextPlayerIndex = _playerListCircular.MoveNext();
            }
            else
            {
                nextPlayerIndex = _playerListCircular.MovePrevious();

                if (skip)
                    nextPlayerIndex = _playerListCircular.MovePrevious();
            }

            //if a miss, reset that flag on deck
            if (skip)
                _gameState.Deck.NextTurnSkip = false;

            //reset uno flag for next round
            _gameState.Deck.CalledUno = false; 

            //change the current player to the next one (calculated above). 
            _gameState.CurrentClientIdTurn = _callbacks.ElementAt(nextPlayerIndex).Key;

            Player currPlayer = _gameState.Players[nextPlayerIndex];

            //does the next player need to draw cards? 
            if (_gameState.Deck.NextTurnPickup > 0)
            {
                for (int i = 0; i < _gameState.Deck.NextTurnPickup; i++)
                    currPlayer.DrawCard(ref _gameState.Deck);
            }

            //reset the cards to add flag
            _gameState.Deck.NextTurnPickup = 0;

            _gameState.AddLogMessage($"It's now {currPlayer.Username} turn!");
        }

        public void DrawCardAndEndTurn()
        {
            DrawCard(1);
            DetermineNextTurn();
            UpdateAllPlayersGameState();
        }
    }
}
