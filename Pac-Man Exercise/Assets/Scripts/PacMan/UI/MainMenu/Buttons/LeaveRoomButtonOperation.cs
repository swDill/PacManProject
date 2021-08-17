using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    [RequireComponent(typeof(Button))]
    public class LeaveRoomButtonOperation : ButtonOperation
    {
        protected override void OnClicked()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}