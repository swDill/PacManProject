using System;
using PacMan.Utility.DependencyInjection;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A button operation to navigate to a given screen when pressed
     */
    public class OpenScreenButtonOperation : ButtonOperation
    {
        [SerializeField] private Screen _screenToShow;
        [Injectable, NonSerialized] public ScreenManager _screenManager;
        
        private void Start()
        {
            DependencyInjection.RequestDependencies(this);
        }

        protected override void OnClicked()
        {
            if (_screenToShow == null)
            {
                Debug.LogError("Cannot open a null screen.", this);
                return;
            }

            if (_screenManager == null)
            {
                Debug.LogError("Screen manager is null.");
            }
            
            _screenManager.OpenScreen(_screenToShow);
        }
    }
}