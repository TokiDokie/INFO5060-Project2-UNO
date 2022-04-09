﻿/* File Name:       WaitingListUpdatedEventArgs.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Wednesday, April 6, 2022
 * Brief:           Insert brief here... */

using System;
using System.Collections.Generic;
using UNOLibrary;

namespace UNOGuiClient.WCF
{
    public class WaitingListUpdatedEventArgs : EventArgs
    {
        public List<Player> Players { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }

        public WaitingListUpdatedEventArgs(List<Player> players, int minPlayers, int maxPlayers)
        {
            Players = players;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
        }
    }
}
