using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PacMan.Entities
{
    /*
     * Player input controller. This controller handles the players inputs utilizing Unitys new Input System.
     */
    public class PlayerInputController : MonoBehaviourPun
    {
        [SerializeField] private InputAction _moveInputAction;

        public event Action<Vector2> InputsUpdated;

        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            _moveInputAction.Enable();
            _moveInputAction.performed += InvokeCallbacks;
        }
        
        private void OnDisable()
        {
            _moveInputAction.performed -= InvokeCallbacks;
            _moveInputAction.Disable();
        }
        
        private void InvokeCallbacks(InputAction.CallbackContext callbackContext)
        {
            if (!_player.IsLocalPlayer) return;
            if (_player.IsDead) return;

            Vector2 direction = callbackContext.ReadValue<Vector2>();
            Vector2 directionAbs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
            
            // Clamp the directions
            if (directionAbs.y >= directionAbs.x)
            {
                direction.x = 0;
            }
            else if (directionAbs.x >= directionAbs.y)
            {
                direction.y = 0;
            }
            
            InputsUpdated?.Invoke(direction);
        }
    }
}