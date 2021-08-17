using System;
using PacMan.Utility.DependencyInjection;
using Photon.Pun;

namespace PacMan.Entities
{
    /*
     * A Base Pellet class to allow us to have shared functionality between all variations of pellets in the game. 
     */
    public abstract class BasePellet : MonoBehaviourPun, IEntity
    {
        [Injectable, NonSerialized] public GameController _gameController;
        
        public bool IsHidden { get; protected set; }
        public bool AbleToRespawn { get; set; } = true;
        
        private void Start()
        {
            DependencyInjection.RequestDependencies(this);
            _gameController.AddPellet(this);
        }

        // Handle being consumed logic
        public void OnConsumed(IEntity consumer)
        {
            if (!(consumer is Player player)) return;

            photonView.RPC(nameof(RequestToHide), RpcTarget.Others, !(this is AttackPellet));
            Hide();

            // Only apply effects if the player who consumed this is local.  
            if (player.photonView.OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ApplyEffects(player);
            }
        }

        // Abstract call for applying effects to the player who consumed the pellet
        protected abstract void ApplyEffects(Player consumer);

        // Respawn the pellet
        public void Respawn()
        {
            if (!AbleToRespawn)
            {
                return;
            }

            IsHidden = false;
            gameObject.SetActive(true);
        }

        // Hide the pellet on all clients
        [PunRPC]
        protected void RequestToHide(bool ableToRespawn)
        {
            AbleToRespawn = ableToRespawn;
            Hide();
        }

        // Hide the pellet so it can be reused later
        
        public void Hide()
        {
            IsHidden = true;
            gameObject.SetActive(false);
            
            _gameController.OnPelletHidden(this);
        }
    }
}