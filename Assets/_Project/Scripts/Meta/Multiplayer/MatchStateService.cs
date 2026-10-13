using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using Zenject;

namespace FirstProject.Meta.Multiplayer
{
    public class MatchStateService : IInRoomCallbacks, IInitializable, IDisposable
    {
        public event Action<bool> WaitingForOpponent;
        public event Action OpponentDisconnected;
        public event Action AllPlayersReady;
        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
            CheckPlayersCount();
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            CheckPlayersCount();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            OpponentDisconnected?.Invoke();
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PhotonMultiplayerService.READY_PROPERTY))
            {
                CheckAllPlayersReady();
            }
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            
        }

        private void CheckAllPlayersReady()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                return;
            }

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(PhotonMultiplayerService.READY_PROPERTY, out object isReadyObj))
                {
                    bool isReady = (bool)isReadyObj;

                    if (!isReady)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            AllPlayersReady?.Invoke();
        }

        private void CheckPlayersCount()
        {
            if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null)
            {
                return;
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                WaitingForOpponent?.Invoke(true);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                WaitingForOpponent?.Invoke(false);
            }
        }
    }
}
