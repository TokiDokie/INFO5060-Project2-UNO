/* File Name:       GameWindow.xaml.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

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
            _changeCardColour = string.Empty;
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

                bool isMyTurn = _gameState.CurrentClientIdTurn == _unoGameClient.ClientId;

                PlayCardButton.IsEnabled = false;
                PickupCardButton.IsEnabled = isMyTurn;

                PlayersHandListBox.Items.Clear();
                ActivePlayersListBox.Items.Clear();

                EventLogsTextBox.Clear();

                foreach ((DateTime timestamp, string log) in _gameState.LogList)
                {
                    EventLogsTextBox.AppendText($"[{timestamp}]: {log}");
                }

                Title = isMyTurn ? $"{_initialWindowTitle} [YOUR TURN]" : _initialWindowTitle;
                YourTurnLabel.Visibility = isMyTurn ? Visibility.Visible : Visibility.Hidden;

                Bitmap topDiscardBitmap = GetUnoCard(_gameState.Deck.TopDiscard.ToString());
                TopCardImage.Source = ImageSourceFromBitmap(topDiscardBitmap);

                foreach (Player player in _gameState.Players)
                {
                    ActivePlayersListBox.Items.Add(string.Format("{0} {1} ({2} card(s) in hand)",
                        player.ClientId == _gameState.CurrentClientIdTurn ? ">" : "",
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
                Console.WriteLine("Error in PlayCardButton_Click");
                return;
            }

            bool cardPlayed = _unoGameClient.TryPlayCard(_currentSelectedCard, _changeCardColour);

            if (!cardPlayed)
            {
                MessageBox.Show("That card cannot be played!", "Player Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PickupCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameState.CurrentClientIdTurn != _unoGameClient.ClientId)
            {
                MessageBox.Show("It's not your turn.", "Information", MessageBoxButton.OK, MessageBoxImage.Information); // Or do something else
                return;
            }

            _unoGameClient.PickUpCard();
        }

        private void CallUnoButton_Click(object sender, RoutedEventArgs e)
        {
            _unoGameClient.CallUno();
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

        #endregion

        #region Unmanaged Code

        // Code in this region modified was borrowed from:
        // https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
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
