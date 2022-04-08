using System.ServiceModel;

namespace UNOLibrary.Networking
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void Update(Deck deck, int nextClient, bool isGameOver);
    }
}
