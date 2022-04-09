/* File Name:       MainWindow.xaml.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:           Main window which is the first window that appears when the program first launches. This window
 *                  allows clients to join the Uno Game's lobby and see other clients in the lobby. When the minimum
 *                  player count is reached, any player can press the "Start Game" button which will start the game on
 *                  a new window. Once a game has started, no new players can join the lobby. */

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using UNOGuiClient.WCF;

namespace UNOGuiClient.Windows
{
    public partial class MainWindow : Window
    {
        UnoGameClient _gameClientInstance;
        GameWindow _gameWindow;

        public MainWindow()
        {
            InitializeComponent();

            _gameClientInstance = UnoGameClient.GetInstance();

            try
            {
                _gameClientInstance.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to connect to the server: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SubscribeToEvents();
        }

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_gameClientInstance.ClientId != -1 && _gameWindow is null)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to close the window? You are currently connected to the lobby.",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _gameClientInstance.LeaveGame();
        }

        private void SubscribeToEvents()
        {
            _gameClientInstance.OnWaitingPlayersUpdated += UnoGameClient_OnWaitingPlayersUpdated;
            _gameClientInstance.OnErrorOccurred += UnoGameClient_OnErrorOccurred;
            _gameClientInstance.OnNewGameStarted += GameClientInstance_OnNewGameStarted;
        }

        private void GameClientInstance_OnNewGameStarted()
        {
            Dispatcher.Invoke(() =>
            {
                _gameWindow = new GameWindow(this, UsernameTextBox.Text);

                Hide();
                _gameWindow.Show();
            });
        }

        private void UnoGameClient_OnWaitingPlayersUpdated(WaitingListUpdatedEventArgs args)
        {
            // NOTE: Using Dispatcher.Invoke() because the events fired by the UnoGameClient are invoked on a seperate thread.
            //       The code below prevents the app from freezing or throwing a runtime exception.
            Dispatcher.Invoke(() =>
            {
                int waitingForTotal = args.MinPlayers - args.Players.Count;

                if (waitingForTotal < 0)
                {
                    waitingForTotal = 0;
                }

                WaitingForPlayersLabel.Visibility = Visibility.Visible;
                WaitingForPlayersLabel.Content = waitingForTotal > 0
                    ? $"Waiting for {waitingForTotal} more player(s)..."
                    : "Ready to start game!";

                // Disable these two components because this method only gets called every time a user
                // joins/leaves the lobby
                UsernameTextBox.IsEnabled = false;
                JoinGameLobbyButton.IsEnabled = false;

                StartGameButton.IsEnabled = true;

                PlayersInLobbyListBox.Items.Clear();

                TotalPlayersInLobbyLabel.Visibility = Visibility.Visible;
                TotalPlayersInLobbyLabel.Content = $"Total: {args.Players.Count}/{args.MaxPlayers} player(s)";

                for (int i = 0; i < args.Players.Count; i++)
                {
                    string username = args.Players[i].Username;

                    PlayersInLobbyListBox.Items.Add(username);
                }
            });
        }

        public void UnoGameClient_OnErrorOccurred(string errorMessage)
        {
            Dispatcher.Invoke(() =>
            {
                // Use Task.Run so that the message box does not block the main thread
                _ = Task.Run(() =>
                {
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            });
        }

        #endregion

        private void JoinGameLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UnoGameClient unoGameClient = UnoGameClient.GetInstance();
                _ = unoGameClient.JoinGame(UsernameTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to join the game lobby: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _gameClientInstance.TryStartGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to start the game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
