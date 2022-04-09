/* File Name:       ICallback.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System.ServiceModel;
using UNOLibrary.Networking.Entities;

namespace UNOLibrary.Networking
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateWaitingList(PlayerList players);

        [OperationContract(IsOneWay = true)]
        void StartNewGame();

        [OperationContract(IsOneWay = true)]
        void UpdateGameState(GameState gameState);

        [OperationContract(IsOneWay = true)]
        void SomeoneLeftGameInProgress(string username);

        [OperationContract(IsOneWay = true)]
        void DisplayErrorMessage(string errorMessage);
    }
}
