using PacMan.Entities;
using Photon.Pun;
using UnityEngine;

namespace PacMan
{
    /*
     * Component to store information about a spawn point, like the correct direction to start in.
     */
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Vector2 _initialDirection;

        public Vector3 Location => transform.position;
        public Vector2 InitialDirection => _initialDirection;

        public void CreatePlayer(GameObject playerPrefab)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Location, Quaternion.identity);
            PlayerDirectionController directionController = newPlayer.GetComponent<PlayerDirectionController>();
            directionController.SetDirection(InitialDirection);
        }
    }
}