using System;
using PacMan.Utility.DependencyInjection;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * Base screen class that handles opening and closing of the screen 
     */
    public class Screen : MonoBehaviour
    {
        [SerializeField] private bool _startOpen = false;
        [Injectable, NonSerialized] public ScreenManager _screenManager;
        
        private void Start()
        {
            DependencyInjection.RequestDependencies(this);

            Close();
            
            if (_screenManager == null)
            {
                Debug.LogError("Screen manager is null");
                return;
            }
                
            _screenManager.OpenScreen(this);
        }

        // Close the screen
        public void Close()
        {
            gameObject.SetActive(false);
        }

        // Open the screen
        public void Open()
        {
            gameObject.SetActive(true);
        }
    }
}