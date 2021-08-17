using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * Pretty simple class, just handles the players color depending if they are p1 or p2 (master or remote, respectively)
     */
    public class PlayerColorController : MonoBehaviourPun
    {
        [SerializeField] private SpriteRenderer _playerSprite;
        [SerializeField] private Color _player1Color;
        [SerializeField] private Color _player2Color;

        private void Awake()
        {
            SetPlayerColor();
        }
        
        private void SetPlayerColor()
        {
            if (_playerSprite == null) return;

            Color color = photonView.Owner.IsMasterClient ? _player1Color : _player2Color;
            _playerSprite.color = color;
        }
    }
}