using System;
using PacMan.Systems;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A structure to define a message for specific network status
     */
    [Serializable]
    public struct NetworkStatusLabelValue
    {
        [SerializeField] private NetworkStatusType _statusType;
        [SerializeField] private string _statusMessage;

        public NetworkStatusType StatusType => _statusType;
        public string StatusMessage => _statusMessage;
    }
}