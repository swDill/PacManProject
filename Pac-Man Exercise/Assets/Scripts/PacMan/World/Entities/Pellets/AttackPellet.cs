using UnityEngine;

namespace PacMan.Entities
{
    /*
     * Handle logic for the attack pellet, once eaten it will make the player enter their attack mode. Thus causing the other remaining attack pellets to hide.
     */
    public class AttackPellet : BasePellet
    {
        [Tooltip("Enable attack timer on entity when consumed by applicable entity, time is in seconds.")]
        [SerializeField] private float _attackTime = 15f;

        // Apply the effects to the consumer
        protected override void ApplyEffects(Player consumer)
        {
            PlayerAttackModeController attackModeController = consumer.GetComponent<PlayerAttackModeController>();

            if (attackModeController == null) return;

            AbleToRespawn = false;
            attackModeController.EnterAttackMode(_attackTime);
        }
    }
}