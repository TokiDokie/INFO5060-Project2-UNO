/* File Name:       PlayerList.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UNOLibrary.Networking.Entities
{
    [DataContract]
    public class PlayerList
    {
        [DataMember]
        public List<Player> Players { get; set; }

        [DataMember]
        public int MinPlayers { get; set; }

        [DataMember]
        public int MaxPlayers { get; set; }

        public PlayerList(List<Player> players, int minPlayers, int maxPlayers)
        {
            Players = players;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
        }
    }
}
