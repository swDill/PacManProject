using System;
using System.Collections;
using PacMan.Utility.DependencyInjection;
using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * The players attack mode controller, handles the players attack mode and when to exit out of it once it runs dry.
     */
    public class PlayerAttackModeController : MonoBehaviourPun
    {
        [Injectable, NonSerialized] public GameController _gameController;
        
        public bool IsAttackModeActive { get; private set; }

        private void Awake()
        {
            DependencyInjection.RequestDependencies(this);
        }

        public void EnterAttackMode(float duration)
        {
            Debug.Log("Attack Mode Activated");
            IsAttackModeActive = true;
            
            StartCoroutine(AttackModeRoutine(duration));
            _gameController.OnPlayerAttackModeStarted(duration);
        }

        private IEnumerator AttackModeRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            Debug.Log("Attack Mode Deactivated");
            IsAttackModeActive = false;
        }
    }
}