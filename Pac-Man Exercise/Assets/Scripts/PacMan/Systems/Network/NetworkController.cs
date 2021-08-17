using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PacMan.Systems
{
    public static class NetworkController
    {
        private static NetworkCallbacks _callbacks;
        private static NetworkStatus _netStatus;
        private static bool _isReconnecting = false;
        
        // Attempt to connect automatically near the start of the applications lifecycle
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            AttemptToConnectToServer();
            _netStatus.UpdateStatus(NetworkStatusType.ConnectingToServer);
        }

        // Attempt to connect to the local server
        private static void AttemptToConnectToServer()
        {
            _callbacks = new NetworkCallbacks();
            _netStatus = new NetworkStatus();
            
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AddCallbackTarget(_callbacks);
            AttemptToConnectLoop();
            
            Debug.Log("Attempting to connect to server..");
        }

        // Start matchmaking, if no rooms are found then automatically create a room with the given settings
        public static void StartMatchmaking()
        {
            _netStatus.UpdateStatus(NetworkStatusType.SearchingForMatch);

            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 2,
                CleanupCacheOnLeave = false
            };

            PhotonNetwork.JoinRandomOrCreateRoom(null, 2, MatchmakingMode.FillRoom, null, null, null, roomOptions, null);
        }

        // Invoke the start of the game when two players have joined 
        public static void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            PhotonNetwork.CurrentRoom.IsOpen = false;
            SceneManager.LoadScene("GameScene");
        }

        // If a player loses connection, causing them to leave prematurely. Then attempt to reconnect.
        public static void InvokeReconnect()
        {
            NetworkCallbacks.LeftRoom -= InvokeReconnect;
            _isReconnecting = true;
            AttemptToConnectLoop();
        }
        
        // Simple attempt to connect loop. Will keep trying if we are unable to connect to the server in prior attempts.
        private static async void AttemptToConnectLoop()
        {
            do
            {
#if UNITY_EDITOR
                // Safeguard to prevent the async code from attempting to connect outside of playmode
                if (!EditorApplication.isPlaying)
                {
                    break;
                }
#endif

                if (_isReconnecting)
                {
                    if (PhotonNetwork.ReconnectAndRejoin())
                    {
                        _isReconnecting = false;
                        break;
                    }
                }
                else
                {
                    if (PhotonNetwork.ConnectUsingSettings()) break;
                }

                await Task.Delay(1000);
            }
            while (!PhotonNetwork.IsConnected);
        }
    }
}