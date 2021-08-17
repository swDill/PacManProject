using System;
using PacMan.Systems;
using PacMan.Utility.DependencyInjection;
using UnityEngine;

namespace PacMan.Entities
{
    /*
     * The score pellet, once consumed then add the pellets score value to the player.
     */
    public class ScorePellet : BasePellet
    {
        [SerializeField] private int _pointValue = 1;
        [Injectable, NonSerialized] public PointController _pointController;

        // Apply the effects to the consumer
        protected override void ApplyEffects(Player consumer)
        {
            if (_pointController == null) return;
            
            _pointController.AwardPoints(consumer, _pointValue);
        }
    }
}