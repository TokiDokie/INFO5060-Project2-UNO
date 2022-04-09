/* File Name:       GameState.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UNOLibrary.Networking.Entities
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public int CurrentClientIdTurn { get; set; }

        [DataMember]
        public Deck Deck;

        [DataMember]
        public List<Player> Players { get; set; }

        [DataMember]
        public List<(DateTime, string)> LogList { get; set; }

        [DataMember]
        public bool GameEnded { get; set; }

        public GameState(int currentClientIdTurn, Deck deck, List<Player> players)
        {
            CurrentClientIdTurn = currentClientIdTurn;
            Deck = deck;
            Players = players;
            LogList = new List<(DateTime, string)>();
        }

        public void AddLogMessage(string message)
        {
            LogList.Add((DateTime.Now, message));
        }
    }
}
