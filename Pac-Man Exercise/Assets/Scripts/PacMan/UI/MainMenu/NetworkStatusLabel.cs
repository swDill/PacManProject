using System.Linq;
using PacMan.Systems;
using TMPro;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A class to visualize on the screen the current state of the network connection to the end-user.
     */
    public class NetworkStatusLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusLabel;
        [SerializeField] private NetworkStatusLabelValue[] _values;
        
        private void Awake()
        {
            NetworkStatus.StatusChanged += RefreshStatusMessage;
        }

        private void OnDestroy()
        {
            NetworkStatus.StatusChanged -= RefreshStatusMessage;
        }

        private void RefreshStatusMessage(NetworkStatusType newStatusType)
        {
            if (_values.All(value => value.StatusType != newStatusType)) return;
            
            NetworkStatusLabelValue newLabelValue = _values.First(value => value.StatusType == newStatusType);

            _statusLabel.text = newLabelValue.StatusMessage;
        }
    }
} 