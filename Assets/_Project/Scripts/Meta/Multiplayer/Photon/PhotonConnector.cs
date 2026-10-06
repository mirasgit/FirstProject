using UnityEngine;
using Photon.Pun;

namespace FirstProject.Meta.Multiplayer
{
    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        public void Initialize()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to Master");
        }
    }
}
