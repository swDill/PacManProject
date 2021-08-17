using System;

namespace PacMan.Systems
{
    /*
     * A controller like class to handle our current network state
     */
    public class NetworkStatus
    {
        public static event Action<NetworkStatusType> StatusChanged;
        
        public NetworkStatus()
        {
            NetworkCallbacks.Connected += OnConnected;
            NetworkCallbacks.Disconnected += OnDisconnected;
            NetworkCallbacks.JoinedRoom += OnJoinedRoom;
            NetworkCallbacks.LeftRoom += OnLeftRoom;
        }

        ~NetworkStatus()
        {
            NetworkCallbacks.Connected -= OnConnected;
            NetworkCallbacks.Disconnected -= OnDisconnected;
            NetworkCallbacks.JoinedRoom -= OnJoinedRoom;
            NetworkCallbacks.LeftRoom -= OnLeftRoom;
        }
        
        private void OnConnected()
        {
            UpdateStatus(NetworkStatusType.InLobby);
        }
        
        private void OnDisconnected()
        {
            UpdateStatus(NetworkStatusType.Disconnected);
        }
        
        private void OnJoinedRoom()
        {
            UpdateStatus(NetworkStatusType.InRoom);
        }

        private void OnLeftRoom()
        {
            UpdateStatus(NetworkStatusType.InLobby);
        }

        public void UpdateStatus(NetworkStatusType newStatus)
        {
            StatusChanged?.Invoke(newStatus);
        }
    }
}