using System;
using PacMan.Systems;
using PacMan.Utility.DependencyInjection;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A script to automatically open a screen when our network status has changed to a defined state. For example, entering a room, disconnecting, etc.
     */
    [RequireComponent(typeof(Screen))]
    public class OpenOnStatusUpdate : MonoBehaviour
    {
        [SerializeField] private NetworkStatusType _targetStatus;
        [Injectable, NonSerialized] public ScreenManager _screenManager;
        
        private Screen _screen;
        
        private void Awake()
        {
            DependencyInjection.RequestDependencies(this);
            
            NetworkStatus.StatusChanged += OnNetworkStatusChanged;
            _screen = GetComponent<Screen>();
        }

        private void OnDestroy()
        {
            NetworkStatus.StatusChanged -= OnNetworkStatusChanged;
        }

        private void OnNetworkStatusChanged(NetworkStatusType newStatus)
        {
            if (_targetStatus != newStatus) return;
            if (_screenManager == null) return;
                
            _screenManager.OpenScreen(_screen);
        }
    }
}