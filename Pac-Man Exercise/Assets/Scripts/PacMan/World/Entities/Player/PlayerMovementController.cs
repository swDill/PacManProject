using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * The player movement controller handles the players movement on screen, simple as it is now. Just move forward in their current direction. 
     */
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviourPun
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private Player _player;
        private PlayerDirectionController _directionController;
        private PlayerAnimationController _animationController;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _player = GetComponent<Player>();
            _directionController = GetComponent<PlayerDirectionController>();
            _animationController = GetComponent<PlayerAnimationController>();
        }

        private void Start()
        {
            _animationController.PlayAliveAnimation();
        }

        private void FixedUpdate()
        {
            if (!_player.IsLocalPlayer) return;
            if (_player.IsDead) return;
            if (_directionController == null) return;

            // Move using physics
            Vector2 translation = _directionController.CurrentDirection * _speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + translation);
        }
    }
}