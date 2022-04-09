/* File Name:       UnoGameClient.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Wednesday, April 6, 2022
 * Brief:           Non-generic class which holds the client callback implementation for the WCF Uno Game. This class
 *                  allows a client to connect to the server, subscribe to game events, and invoke methods on the server. */

using System.ServiceModel;
using UNOLibrary.Networking;
using UNOLibrary.Networking.Entities;

namespace UNOGuiClient.WCF
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class UnoGameClient : ICallback
    {
        private static UnoGameClient _instance;

        public int ClientId { get; private set; }
        public bool IsGameStarted { get; private set; }
        public bool IsGameOver { get; private set; }

        private IUnoGame _unoGame;

        public event WaitingPlayersUpdatedEventHandler OnWaitingPlayersUpdated;
        public delegate void WaitingPlayersUpdatedEventHandler(WaitingListUpdatedEventArgs args);

        public event NewGameStartedEventHandler OnNewGameStarted;
        public delegate void NewGameStartedEventHandler();

        public event GameStateUpdatedEventHandler OnGameStateUpdated;
        public delegate void GameStateUpdatedEventHandler(GameState gameState);

        public event ErrorOccurredEventHandler OnErrorOccurred;
        public delegate void ErrorOccurredEventHandler(string errorMessage);

        public event SomeoneLeftGameInProgressEventHandler OnSomeoneLeftGameInProgress;
        public delegate void SomeoneLeftGameInProgressEventHandler(string username);

        private UnoGameClient()
        {
            ClientId = -1;
            IsGameStarted = false;
            IsGameOver = false;
        }

        public static UnoGameClient GetInstance()
        {
            if (_instance is null)
            {
                _instance = new UnoGameClient();
            }

            return _instance;
        }

        public void Connect()
        {
            DuplexChannelFactory<IUnoGame> channel = new DuplexChannelFactory<IUnoGame>(this, "UnoGuiClientEndpoint");
            _unoGame = channel.CreateChannel();
        }

        public bool JoinGame(string username)
        {
            if (!(_unoGame is null))
            {
                int clientId = _unoGame.TryJoinLobby(username);

                if (clientId != -1)
                {
                    ClientId = clientId;
                }

                _unoGame.UpdateWaitingList();
            }
            
            return ClientId >= 0;
        }

        public void LeaveGame()
        {
            _unoGame?.LeaveGame();
        }

        public void TryStartGame()
        {
            _unoGame?.TryStartGame();
        }

        public bool TryPlayCard(string card, string changeCardColour)
        {
            return _unoGame?.TryPlayCard(card, changeCardColour) ?? false;
        }

        public void PickUpCard()
        {
            _unoGame?.DrawCardAndEndTurn();
        }

        public void CallUno()
        {
            _unoGame?.CallUno();
        }

        public void DisplayErrorMessage(string errorMessage)
        {
            OnErrorOccurred?.Invoke(errorMessage);
        }

        public void UpdateWaitingList(PlayerList playerList)
        {
            OnWaitingPlayersUpdated?.Invoke(new WaitingListUpdatedEventArgs(playerList.Players,
                playerList.MinPlayers,
                playerList.MaxPlayers));
        }

        public void StartNewGame()
        {
            OnNewGameStarted?.Invoke();
        }

        public void UpdateGameState(GameState gameState)
        {
            OnGameStateUpdated?.Invoke(gameState);
        }

        public void SomeoneLeftGameInProgress(string username)
        {
            OnSomeoneLeftGameInProgress?.Invoke(username);
        }
    }
}
