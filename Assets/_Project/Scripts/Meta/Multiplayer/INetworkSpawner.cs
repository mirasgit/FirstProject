using UnityEngine;

namespace FirstProject.Meta.Multiplayer
{
    public interface INetworkSpawner
    {
        GameObject Instantiate(string prefabID, Vector3 position, Quaternion rotation, object[] data = null);

        void Destroy(GameObject target);
    }
}
