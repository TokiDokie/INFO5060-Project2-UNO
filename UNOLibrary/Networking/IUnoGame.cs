using System.ServiceModel;

namespace UNOLibrary.Networking
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IUnoGame
    {
        [OperationContract]
        int JoinGame();

        [OperationContract(IsOneWay = true)]
        void LeaveGame();
    }
}
