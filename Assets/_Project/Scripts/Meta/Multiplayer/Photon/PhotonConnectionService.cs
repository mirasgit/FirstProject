using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FirstProject.Meta.Multiplayer
{
    public class PhotonConnectionService : IInitializable, IDisposable, IConnectionCallbacks, IMatchmakingCallbacks
    {
        public event Action ConnectedToMaster;
        public event Action JoinedRoom;
        public event Action<string> RoomOperationFailed; 

        public void Initialize()
        {
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void CreateRoom(string roomID)
        {
            if (string.IsNullOrEmpty(roomID)) return;

            RoomOptions options = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomID, options);
        }

        public void ConnectToRoom(string roomID)
        {
            if (string.IsNullOrEmpty(roomID)) return;

            PhotonNetwork.JoinRoom(roomID);
        }

        public void OnJoinedRoom()
        {
            JoinedRoom?.Invoke();
            Debug.Log("Joined Room");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("BattleSimulation");
            }
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            RoomOperationFailed?.Invoke(message);
            Debug.LogWarning($"Join Failed: {message}");
        }

        public void OnCreatedRoom() { }
        public void OnCreateRoomFailed(short returnCode, string message)
        {
            RoomOperationFailed?.Invoke(message);
        }
        public void OnJoinRandomFailed(short returnCode, string message) { }
        public void OnLeftRoom() { }
        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master Server!");
            ConnectedToMaster?.Invoke();
        }

        public void OnConnected() { }
        public void OnDisconnected(DisconnectCause cause) { }
        public void OnRegionListReceived(RegionHandler regionHandler) { }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public void OnCustomAuthenticationFailed(string debugMessage) { }
    }
}