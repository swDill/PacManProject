using System;
using PacMan.Utility.DependencyInjection;
using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * This is the main player class, it handles the basics like death and respawning, Life count, etc. There are sub controllers for the player to handle other
     * aspects like moving and animation. It is better to have them split up so one script or "Controller" does one function, in compliance with the SOLID principles.
     * Also having a long, cluttered script is just horrible to deal with.
     *
     * If I were to expand on this I would have setup a base class to handle retrieving the components rather then needing to do a GetComponent call a ton of times.
     */
    public class Player : MonoBehaviourPun, IEntity
    {
        public static event Action<bool, int> PlayerDeath;
        
        [SerializeField] private int _startingLives = 3;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Injectable, NonSerialized] public GameController _gameController;
        
        private bool _isDead;
        private int _currentLifeCount;
        private CircleCollider2D _collider;
        private PlayerAnimationController _animationController;
        private PlayerDirectionController _directionController;
        private PlayerAttackModeController _attackModeController;

        public bool IsDead => _isDead;
        public CircleCollider2D Collider => _collider;
        public bool IsLocalPlayer => photonView.IsMine && photonView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber;

        private void Awake()
        {
            CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
            
            foreach (CircleCollider2D col in colliders)
            {
                if (!col.isTrigger)
                {
                    _collider = col;
                }
            }
            
            _animationController = GetComponent<PlayerAnimationController>();
            _directionController = GetComponent<PlayerDirectionController>();
            _attackModeController = GetComponent<PlayerAttackModeController>();
        }

        private void Start()
        {
            DependencyInjection.RequestDependencies(this);
            
            _currentLifeCount = _startingLives;

            // Ensure that the remote player is always below the local player on their own screen.
            if (!IsLocalPlayer)
            {
                _spriteRenderer.sortingOrder = -1;
            }

            _gameController.AddPlayer(this);
        }

        // Handle trigger events, against pellets or other players
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsLocalPlayer) return;
            
            IEntity entity = other.gameObject.GetComponent<IEntity>();

            if (entity == null) return;
            
            TryAndConsume(entity);
        }

        // Attempt to consume another entity
        private void TryAndConsume(IEntity otherEntity)
        {
            // Do not consume a player if our own attack mode is not enabled.
            if (otherEntity is Player && !_attackModeController.IsAttackModeActive) return;
            
            otherEntity.OnConsumed(this);
        }

        // Handle the player being consumed
        public void OnConsumed(IEntity consumer)
        {
            // Prevent a player from being consumed if they are already dead.
            if (_isDead) return;
            
            Debug.Log($"Player \"{ name }\" has been consumed.");
            photonView.RPC(nameof(Die), RpcTarget.All);
        }
        
        // Handle the player death
        [PunRPC]
        private void Die()
        {
            // Prevent a player from dying if they are already dead.
            if (_isDead) return;
            
            Debug.Log($"Player \"{ name }\" has died.");
            
            _isDead = true;
            _currentLifeCount--;
            Action deathCallback = _currentLifeCount > 0 ? (Action) Respawn : Disqualify;
            _directionController.SetDirection(Vector2.right);
            _animationController.PlayDeathAnimation(deathCallback);
            
            PlayerDeath?.Invoke(this, _currentLifeCount);
        }

        // Request this player be respawned
        private void Respawn()
        {
            Debug.Log($"Player \"{ name }\" respawned.");
            
            SpawnPoint spawnPoint = _gameController.GetSpawnFurthestFromOther(this);
            transform.position = spawnPoint.Location;
            _directionController.SetDirection(spawnPoint.InitialDirection);
            _animationController.PlayAliveAnimation();
            _isDead = false;
        }

        // Request this player be disqualified
        private void Disqualify()
        {
            _gameController.DisqualifyPlayer(this);
            gameObject.SetActive(false);
        }
    }
}