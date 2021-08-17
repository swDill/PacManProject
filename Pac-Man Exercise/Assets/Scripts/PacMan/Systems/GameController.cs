using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PacMan.Entities;
using PacMan.Systems;
using PacMan.Utility.DependencyInjection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PacMan
{
    /*
     * The central game controller, handles the game flow and pellets in the scene.
     */
    public class GameController : MonoBehaviourPun
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private float _respawnAllAttackPelletsAfter = 5f;
        [SerializeField] private float _respawnAllScorePelletsAfter = 5f;
        [SerializeField] private float _autoForfeitAfterTime = 10f;
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [Injectable, NonSerialized] public PointController _pointController; 

        // List of players, attack pellets and score pellets in the game
        private readonly List<Player> _players = new List<Player>();
        private readonly List<AttackPellet> _attackPellets = new List<AttackPellet>();
        private List<ScorePellet> _scorePellets = new List<ScorePellet>();

        private void Awake()
        {
            DependencyInjection.InjectAsType(this);
            PhotonObjectController.AddPrefabToPool(_playerPrefab.gameObject);

            NetworkCallbacks.LeftRoom += ReturnToMainMenu;
            NetworkCallbacks.LeftRoom += NetworkController.InvokeReconnect;
            NetworkCallbacks.OtherPlayerLeftRoom += OtherPlayerDisconnected;
            NetworkCallbacks.Disconnected += DisconnectToMainMenu;
        }

        private void Start()
        {
            DependencyInjection.RequestDependencies(this);
            CreateLocalPlayer();
        }
        
        private void OnDestroy()
        {
            DependencyInjection.DeleteDependency<GameController>();
            PhotonObjectController.RemovePrefabFromPool(_playerPrefab.gameObject);
            
            NetworkCallbacks.LeftRoom -= ReturnToMainMenu;
            NetworkCallbacks.OtherPlayerLeftRoom -= OtherPlayerDisconnected;
        }

        private void OtherPlayerDisconnected()
        {
            StartCoroutine(ForfeitRoutine());
        }

        // If the other player has disconnected, then automatically declare the remaining player the winner if they have not rejoined.
        private IEnumerator ForfeitRoutine()
        {
            yield return new WaitForSeconds(_autoForfeitAfterTime);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                DeclareWinner(_players.FirstOrDefault(player => player.IsLocalPlayer));
            }
        }

        private void CreateLocalPlayer()
        {
            SpawnPoint spawnPoint = GetDefaultSpawn();
            spawnPoint.CreatePlayer(_playerPrefab.gameObject);
        }

        // Add player to the list of players
        public void AddPlayer(Player player)
        {
            Debug.Log($"Adding player \"{ player.name }\".");
         
            _players.Add(player);

            IgnorePlayerCollisions();
        }

        // Ignore player collisions, this approach allows us to still interact with triggers as well. 
        private void IgnorePlayerCollisions()
        {
            foreach (Player player in _players)
            {
                foreach (Player other in _players)
                {
                    if (other == player) continue;
                    
                    Physics2D.IgnoreCollision(player.Collider, other.Collider, true);
                }
            }
        }

        // Disqualify a player and remove them from the list of players
        public void DisqualifyPlayer(Player player)
        {
            Debug.Log($"Disqualifying player \"{ player.name }\".");
            
            _players.Remove(player);

            switch (_players.Count)
            {
                // If one player is left, they are the winner!
                case 1:
                    DeclareWinner(_players[0]);
                    break;
                // Handle a "draw" scenario just in case.
                case 0:
                    break;
            }
        }

        // Declare a player as the winner
        private void DeclareWinner(Player player)
        {
            if (player != null)
            {
                Debug.Log(player.name + " is the winner!");

                if (player.IsLocalPlayer && _pointController != null)
                {
                    _pointController.CheckForHighscore(player);
                }
            }
            
            PhotonNetwork.LeaveRoom();
        }

        public void AddPellet(BasePellet pellet)
        {
            Debug.Log("Adding Pellet");
            
            switch (pellet)
            {
                case AttackPellet attackPellet:
                    _attackPellets.Add(attackPellet);
                    break;
                case ScorePellet scorePellet:
                    _scorePellets.Add(scorePellet);
                    break;
            }
        }
        
        public SpawnPoint GetSpawnFurthestFromOther(Player currentPlayer)
        {
            Player otherPlayer = _players.FirstOrDefault(player => player != currentPlayer);

            if (otherPlayer == null)
            {
                return GetDefaultSpawn();
            }

            return _spawnPoints.OrderByDescending(spawn => Vector3.Distance(spawn.Location, otherPlayer.transform.position)).First();
        }

        public void OnPlayerAttackModeStarted(float duration)
        {
            photonView.RPC(nameof(HideAttackPellets), RpcTarget.AllViaServer, duration);
        }

        [PunRPC]
        private void HideAttackPellets(float duration)
        {
            foreach (AttackPellet attackPellet in _attackPellets)
            {
                attackPellet.Hide();
            }

            StartCoroutine(ShowAttackPelletsAgain(duration));
        }

        private IEnumerator ShowAttackPelletsAgain(float duration)
        {
            bool allAttackPelletsAreConsumed = _attackPellets.All(attackPellet => !attackPellet.AbleToRespawn);

            yield return new WaitForSeconds(duration);

            if (allAttackPelletsAreConsumed)
            {
                yield return new WaitForSeconds(_respawnAllAttackPelletsAfter);
            }
            
            RespawnAttackPellets(allAttackPelletsAreConsumed);
        }
        
        private void RespawnAttackPellets(bool forceRespawnAll = false)
        {
            foreach (AttackPellet attackPellet in _attackPellets)
            {
                if (forceRespawnAll)
                {
                    attackPellet.AbleToRespawn = true;
                }
                
                attackPellet.Respawn();
            }
        }
        
        // Return to the main menu normally.
        private void ReturnToMainMenu()
        {
            NetworkCallbacks.LeftRoom -= NetworkController.InvokeReconnect;
            DisconnectToMainMenu();
        }
        
        // Disconnect from the game and return to the main menu
        private void DisconnectToMainMenu()
        {
            NetworkCallbacks.Disconnected -= DisconnectToMainMenu;
            SceneManager.LoadScene(0);
        }
        
        private SpawnPoint GetDefaultSpawn()
        {
            return PhotonNetwork.IsMasterClient ? _spawnPoints[0] : _spawnPoints[1];
        }

        // Check for if all the pellets have been consumed, if so then queue to respawn them
        public void OnPelletHidden(BasePellet pelletConsumed)
        {
            if (!(pelletConsumed is ScorePellet)) return;
            
            bool allHidden = _scorePellets.All(pellet => pellet.IsHidden);

            if (!allHidden) return;

            photonView.RPC(nameof(StartScorePelletRespawnRoutine), RpcTarget.AllViaServer);
        }

        [PunRPC]
        private void StartScorePelletRespawnRoutine()
        {
            StartCoroutine(ScorePelletRespawnRoutine());
        }

        private IEnumerator ScorePelletRespawnRoutine()
        {
            yield return new WaitForSeconds(_respawnAllScorePelletsAfter);

            foreach (ScorePellet scorePellet in _scorePellets)
            {
                scorePellet.Respawn();
            }
        }
    }
}