using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities.PlayerNet
{
    /*
     * A custom solution over a PhotonTransformView will give us better control over what data is being sent, And how that data is being processed.
     */
    public class PlayerOrientationView : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private float _positionSharpness;
        [SerializeField] private float _teleportDistance = 3;
        
        private Vector3 _networkPosition;
        private Vector2 _networkDirection;
        private PlayerDirectionController _directionController;
        
        private void Awake()
        {
            _directionController = GetComponent<PlayerDirectionController>();
        }

        public void Update()
        {
            if (photonView.IsMine) return;

            // Automatically teleport if the distance between the network position and the actual position is greater then the given distance
            if (Vector3.Distance(transform.position, _networkPosition) > _teleportDistance)
            {
                transform.position = _networkPosition;
            }
            // Otherwise smoothly move to the network location
            else
            {
                transform.position = Vector3.Lerp(transform.position, _networkPosition, 1f - Mathf.Exp(-_positionSharpness * Time.deltaTime));    
            }
            
            // Update the remote players direction
            _directionController.SetDirection(_networkDirection);
        }
        
        // Netcode to handle transfering data between clients.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(_directionController.CurrentDirection);
            }
            else if (stream.IsReading)
            {
                _networkPosition = (Vector3) stream.ReceiveNext();
                
                Vector2 newDirection = (Vector2) stream.ReceiveNext();
                _networkDirection = newDirection;
            }
        }
    }
}