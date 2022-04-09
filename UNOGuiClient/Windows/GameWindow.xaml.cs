/* File Name:       GameWindow.xaml.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:           Window which encapsulates all logic for the player of an active UNO game. This window shows the state of the game sent
 *                  by the server and allows to user to select cards and perform game moves. */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Collections.Generic;
using UNOGuiClient.WCF;
using UNOLibrary;
using UNOLibrary.Networking.Entities;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using UNOGuiClient.Containers;
using System.Threading.Tasks;

namespace UNOGuiClient.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private UnoGameClient _unoGameClient;
        private MainWindow _mainWindow;
        private GameState _gameState;
        private List<Bitmap> _playerCardImages;

        private string _initialWindowTitle;

        private string _changeCardColour;
        private string _currentSelectedCard;

        public GameWindow(MainWindow mainWindow, string clientUsername)
        {
            _mainWindow = mainWindow;
            _unoGameClient = UnoGameClient.GetInstance();
            _playerCardImages = new List<Bitmap>();

            // NOTE: This is "blue" because the blue colour radio button is checked by default (so if a player doesn't select
            // anything, this is the default colour)
            _changeCardColour = "blue"; //wik

            _currentSelectedCard = null;

            InitializeComponent();
            SubscribeToEvents();

            _initialWindowTitle = Title;

            ClientUsernameLabel.Content = $"Playing As: {clientUsername}";
        }

        #region Event Handlers

        private void SubscribeToEvents()
        {
            _unoGameClient.OnGameStateUpdated += UnoGameClient_OnGameStateUpdated;
            _unoGameClient.OnSomeoneLeftGameInProgress += UnoGameClient_OnSomeoneLeftGameInProgress;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _mainWindow.Close();
        }

        private void UnoGameClient_OnGameStateUpdated(GameState gameState)
        {
            Dispatcher.Invoke(() =>
            {
                _gameState = gameState;

                ColourRadioButtonsContainer.Visibility = Visibility.Hidden; // Force hide this in case someone selects a black card
                EventLogsTextBox.Clear();

                foreach ((DateTime timestamp, string log) in _gameState.LogList)
                {
                    EventLogsTextBox.AppendText($"[{timestamp:HH:mm:ss tt}]: {log}\n");
                }

                EventLogsTextBox.ScrollToEnd();

                if (_gameState.GameEnded) // Game has been won, can only view scores or end session
                {
                    DisableUi();
                }
                else // Game is still ongoing
                {
                    bool isMyTurn = _gameState.CurrentClientIdTurn == _unoGameClient.ClientId;

                    PlayCardButton.IsEnabled = false;
                    PickupCardButton.IsEnabled = isMyTurn;

                    PlayersHandListBox.Items.Clear();
                    ActivePlayersListBox.Items.Clear(); //also wik

                    Title = isMyTurn ? $"{_initialWindowTitle} [YOUR TURN]" : _initialWindowTitle;
                    YourTurnLabel.Visibility = isMyTurn ? Visibility.Visible : Visibility.Hidden;

                    Bitmap topDiscardBitmap = GetUnoCard(_gameState.Deck.TopDiscard.ToString());
                    TopCardImage.Source = ImageSourceFromBitmap(topDiscardBitmap);

                    foreach (Player player in _gameState.Players)
                    {
                        ActivePlayersListBox.Items.Add(string.Format("{0} {1} ({2} card(s) in hand)",
                            player.ClientId == _gameState.CurrentClientIdTurn ? ">" : "", // Show an arrow next to the username that has the current turn
                            player,
                            player.PlayerHand.Count));
                    }

                    Player myPlayer = gameState.Players.Find(player => player.ClientId == _unoGameClient.ClientId);

                    if (!(myPlayer is null))
                    {
                        // Show the players hand

                        foreach (Card card in myPlayer.PlayerHand)
                        {
                            ImageSource faceImageSource = ImageSourceFromBitmap(GetUnoCard(card.ToString()));
                            UnoFaceValuePair unoFaceValuePair = new UnoFaceValuePair(card.ToString(), faceImageSource);

                            PlayersHandListBox.Items.Add(unoFaceValuePair);
                        }
                    }

                    foreach (var card in myPlayer.PlayerHand)
                    {
                        _playerCardImages.Add(GetUnoCard(card.ToString()));
                    }

                    // Try and bring the window to the front if it is the current player's turn
                    if (isMyTurn)
                    {
                        Activate();
                    }
                }
            });
        }

        private void UnoGameClient_OnSomeoneLeftGameInProgress(string username)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    DisableUi(); // NOTE: This is incase the Application fails to force exit

                    _unoGameClient.LeaveGame();

                    MessageBox.Show(this,
                        $"The player \"{username}\" has left the game! The game has forcefully ended.",
                        "Game Forcefully Ended",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }
                catch (Exception)
                {
                    // Do nothing
                }
            });
        }

        private void PlayersHand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || _gameState.CurrentClientIdTurn != _unoGameClient.ClientId)
            {
                _currentSelectedCard = null;
                return;
            }

            UnoFaceValuePair selectedCard = (UnoFaceValuePair)e.AddedItems[0];

            _currentSelectedCard = selectedCard.CardValue;
            ColourRadioButtonsContainer.Visibility = _currentSelectedCard.Contains("black") ? Visibility.Visible : Visibility.Hidden;
            PlayCardButton.IsEnabled = true;
        }

        private void PlayCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSelectedCard is null)
            {
                return;
            }

            try
            {
                bool cardPlayed = _unoGameClient.TryPlayCard(_currentSelectedCard, _changeCardColour);

                if (!cardPlayed)
                {
                    MessageBox.Show("That card cannot be played!", "Player Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to play the card: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PickupCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameState.CurrentClientIdTurn != _unoGameClient.ClientId)
            {
                MessageBox.Show("It's not your turn.", "Information", MessageBoxButton.OK, MessageBoxImage.Information); // Or do something else
                return;
            }

            try
            {
                _unoGameClient.PickUpCard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to pickup a card: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CallUnoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _unoGameClient.CallUno();

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(this, "You called UNO!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to call UNO: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ColourRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is RadioButton))
            {
                return;
            }

            RadioButton clickedRadioBtn = sender as RadioButton;
            _changeCardColour = clickedRadioBtn.Content.ToString().ToLower();
        }

        #endregion

        #region Helper Methods

        public Bitmap GetUnoCard(string cardString)
        {
            ResourceManager resourceManager = new ResourceManager("UNOGuiClient.Properties.Resources", Assembly.GetExecutingAssembly());
            return (Bitmap)resourceManager.GetObject(cardString);
        }

        private void DisableUi()
        {
            PlayCardButton.IsEnabled = false;
            PickupCardButton.IsEnabled = false;
            CallUnoButton.IsEnabled = false;
            PlayersHandListBox.IsEnabled = false;
        }

        #endregion

        #region Unmanaged Code

        // Code in this region modified was borrowed from:
        // https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            // NOTE: If a bitmap is null, GetHbitmap() will throw an exception. Therefore, to avoid the program crashing, just return
            //       null.
            if (bmp is null)
            {
                return null;
            }

            IntPtr handle = bmp.GetHbitmap();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                _ = DeleteObject(handle);
            }
        }

        #endregion
    }
}
