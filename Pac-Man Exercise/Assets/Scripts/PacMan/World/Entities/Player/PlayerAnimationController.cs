using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * Player animation controller, handles the players animation duties in a clean matter.
     */
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviourPun
    {
        [SerializeField] private int _layer = 0;
        [SerializeField] private string _aliveTrigger;
        [SerializeField] private string _deathTrigger;

        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAliveAnimation(Action callback = null)
        {
            ProcessAnimation(_aliveTrigger, callback);
        }
        
        public void PlayDeathAnimation(Action callback = null)
        {
            ProcessAnimation(_deathTrigger, callback);
        }

        // Process a requested animation, if that fails then invoke the supplied callback 
        private void ProcessAnimation(string trigger, Action callback)
        {
            if (trigger == null)
            {
                // A check for a null animation reference, no point in playing it if so. Just invoke the callback immediately and exit.
                callback?.Invoke();
                return;
            }
            
            StartCoroutine(AnimationRoutine(trigger, callback));
        }
        
        private IEnumerator AnimationRoutine(string trigger, Action callback, float extraDelay = 1f)
        {
            _animator.SetTrigger(trigger);
            
            // Wait for the animation to change before invoking the callback
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(_layer).Length);
            yield return new WaitForSeconds(extraDelay);
            
            _animator.ResetTrigger(trigger);
            
            callback?.Invoke();
        }
    }
}