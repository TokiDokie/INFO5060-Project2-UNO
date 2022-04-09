/* File Name:       IUnoGame.cs
 * By:              Darian Benam, Darrell Bryan, Jacob McMullin, and Riley Kipp
 * Date Created:    Tuesday, April 5, 2022
 * Brief:            */

using System.ServiceModel;

namespace UNOLibrary.Networking
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IUnoGame
    {
        [OperationContract]
        int TryJoinLobby(string username);

        [OperationContract(IsOneWay = true)]
        void LeaveGame();

        [OperationContract(IsOneWay = true)]
        void UpdateWaitingList();

        [OperationContract(IsOneWay = true)]
        void TryStartGame();

        [OperationContract]
        bool TryPlayCard(string card, string changeCardColour);

        [OperationContract(IsOneWay = true)]
        void DrawCard(int numCards);

        [OperationContract(IsOneWay = true)]
        void DrawCardAndEndTurn();

        [OperationContract(IsOneWay = true)]
        void CallUno();
    }
}
