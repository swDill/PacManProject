using PacMan.Systems;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A score UI label, listens in on when score is changed and updates on callbacks
     */
    public class ScoreLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textLabel;
        [SerializeField] private PlayerListenType _listeningPlayerListenType;
        
        private void Awake()
        {
            PointController.ScoreUpdated += UpdateScore;
        }

        // Update the score for the correct player when they score a point
        private void UpdateScore(int actorNumber, int newScore)
        {
            if (_textLabel == null) return;

            bool isLocalPlayer = PhotonNetwork.LocalPlayer.ActorNumber == actorNumber; 
            bool listenToLocalPlayer = _listeningPlayerListenType == PlayerListenType.LocalPlayer;
            
            bool isRemotePlayer = PhotonNetwork.LocalPlayer.ActorNumber != actorNumber; 
            bool listenToRemotePlayer = _listeningPlayerListenType == PlayerListenType.RemotePlayer;

            if (isLocalPlayer && listenToLocalPlayer || isRemotePlayer && listenToRemotePlayer)
            {
                _textLabel.text = newScore.ToString();    
            }
        }
    }
}