using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PacMan.Systems
{
    public class NetworkCallbacks : IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        public static event Action Connected;
        public static event Action Disconnected;
        public static event Action JoinedRoom;
        public static event Action LeftRoom;
        public static event Action OtherPlayerLeftRoom;

        public void OnConnected()
        {
            Debug.Log("Connected to server");
            Connected?.Invoke();
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server");
            Connected?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected from server: { cause }");
            Disconnected?.Invoke();
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("Region list received");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.Log("Custom authentication response received");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.LogError($"Custom authentication failed: { debugMessage }");
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            Debug.Log("Friends list updated");
        }

        public void OnCreatedRoom()
        {
            Debug.Log("Created a room");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Failed to create a room, { returnCode }: { message }");
        }

        public void OnJoinedRoom()
        {
            Debug.Log("Joined a room");
            JoinedRoom?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Failed to join a room, { returnCode }: { message }");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogError($"Failed to join a random room, { returnCode }: { message }");
        }

        public void OnLeftRoom()
        {
            Debug.Log("Left the room");
            LeftRoom?.Invoke();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player { newPlayer.ActorNumber } entered the room");
            NetworkController.StartGame();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player { otherPlayer.ActorNumber } left the room");
            OtherPlayerLeftRoom?.Invoke();
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            Debug.Log("Room properties have updated");
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("A player properties have updated");
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.Log($"Master clients have switched, new master client is { newMasterClient.ActorNumber }");
        }
    }
}