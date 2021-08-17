using PacMan.Systems;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    /*
     * A script to enable a button when we connect to the server. Otherwise it is not interactable
     */
    [RequireComponent(typeof(Button))]
    public class EnableButtonOnConnect : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            SetInteractable(PhotonNetwork.IsConnected);
            
            NetworkCallbacks.Connected += OnConnected;
            NetworkCallbacks.Disconnected += OnDisconnected;
        }

        private void OnDestroy()
        {
            NetworkCallbacks.Connected -= OnConnected;
            NetworkCallbacks.Disconnected -= OnDisconnected;
        }

        private void OnConnected()
        {
            SetInteractable(true);
        }
        
        private void OnDisconnected()
        {
            SetInteractable(false);
        }

        // Set the button interactable if it exists
        private void SetInteractable(bool interactable)
        {
            if (_button == null) return;

            _button.interactable = interactable;
        }
    }
}