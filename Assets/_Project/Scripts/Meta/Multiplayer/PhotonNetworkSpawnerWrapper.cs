using Photon.Pun;
using UnityEngine;

namespace FirstProject.Meta.Multiplayer
{
    public class PhotonNetworkSpawnerWrapper : INetworkSpawner
    {
        public GameObject Instantiate(string prefabID, Vector3 position, Quaternion rotation, object[] data = null)
        {
            return PhotonNetwork.Instantiate(prefabID, position, rotation, 0, data);
        }

        public void Destroy(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            PhotonView view = target.GetComponent<PhotonView>();

            if (view != null && view.IsMine)
            {
                PhotonNetwork.Destroy(target);
            }
        }
    }
}
