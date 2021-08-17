using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * Player direction controller, handles what direction the players sprite should be facing.
     */
    public class PlayerDirectionController : MonoBehaviourPun
    {
        [SerializeField] private float _passageCheckScale = 0.9f;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private Vector2 _requestedDirection;
        public Vector2 CurrentDirection { get; private set; }

        private Player _player;
        private PlayerInputController _inputController;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _inputController = GetComponent<PlayerInputController>();
        }

        private void OnEnable()
        {
            if (_inputController != null)
            {
                _inputController.InputsUpdated += RequestNewDirection;
            }
        }

        private void OnDisable()
        {
            if (_inputController != null)
            {
                _inputController.InputsUpdated -= RequestNewDirection;
            }
        }

        // Gizmos to help debug the current direction and requested direction
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.localPosition, CurrentDirection);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.localPosition, _requestedDirection);
        }
        
        private void Update()
        {
            if (!_player.IsLocalPlayer) return;
            if (_requestedDirection == Vector2.zero) return;
            
            // Try to set the players direction when the path is clear to do so
            UpdateDirection();
        }

        private void UpdateSpriteRotation()
        {
            float targetRotation = 0;
            
            // Have the sprite renderer rotate to allow its orientation view to correctly sync the pac mans rotation across clients 
            if (CurrentDirection.x != 0)
            {
                targetRotation = CurrentDirection.x > 0 ? 0 : 180;
            }
            else if (CurrentDirection.y != 0)
            {
                targetRotation = CurrentDirection.y > 0 ? 90 : 270;
            }

            SetRotation(targetRotation);
        }
        
        private void SetRotation(float rotation)
        {
            _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        public void SetDirection(Vector2 direction)
        {
            CurrentDirection = direction;
            _requestedDirection = Vector2.zero;

            UpdateSpriteRotation();
        }

        // Queue up a requested direction, so when it is free to turn, then we can automatically turn. This is prevents the need for the player to do pixel-perfect inputs.
        private void RequestNewDirection(Vector2 direction)
        {
            if (!photonView.IsMine) return;
            
            _requestedDirection = direction;
         
            UpdateDirection();
        }
        
        private void UpdateDirection()
        {
            if (!CheckArea(_requestedDirection)) return;
            
            CurrentDirection = _requestedDirection;
            _requestedDirection = Vector2.zero;
            
            UpdateSpriteRotation();
        }
        
        // Simple check to see if the path in the given direction is clear 
        private bool CheckArea(Vector2 direction)
        {
            // If no collider is hit then there is no obstacle in that direction
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * _passageCheckScale, 0.0f, direction, 1.5f, _wallLayer);
            return hit.collider == null;
        }
    }
}