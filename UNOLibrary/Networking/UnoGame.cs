using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace UNOLibrary.Networking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UnoGame : IUnoGame
    {
        private Dictionary<int, ICallback> _callbacks;
        private Deck _deck;
        private List<Player> _players;

        private int _nextClientId;
        private int _currentTurnId;

        public UnoGame()
        {
            _callbacks = new Dictionary<int, ICallback>();
            _deck = new Deck();
            _players = new List<Player>();
            _nextClientId = 0;
            _currentTurnId = -1;
        }

        public int JoinGame()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(cb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(cb);
                return _callbacks.Keys.ElementAt(callbackIndex);
            }

            _callbacks.Add(_nextClientId, cb);

            return _nextClientId++;
        }

        public void LeaveGame()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (_callbacks.ContainsValue(cb))
            {
                int callbackIndex = _callbacks.Values.ToList().IndexOf(cb);
                int id = _callbacks.ElementAt(callbackIndex).Key;

                _callbacks.Remove(id);
            }
        }

        private void UpdateAllClients()
        {
            foreach (ICallback cb in _callbacks.Values)
            {
                // cb.Update(null, null, false);
            }
        }
    }
}
