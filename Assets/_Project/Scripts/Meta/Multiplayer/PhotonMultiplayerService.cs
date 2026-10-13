using Photon.Pun;
using ExitGames.Client.Photon;

namespace FirstProject.Meta.Multiplayer
{
    public class PhotonMultiplayerService : IMultiplayerService
    {
        public const string READY_PROPERTY = "IsReady";

        public bool InRoom => PhotonNetwork.InRoom;

        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;

        public void SetReadyState(bool ready)
        {
            var hash = new Hashtable { { READY_PROPERTY, ready } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}