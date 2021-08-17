using System;
using System.Collections.Generic;
using PacMan.Entities;
using PacMan.Utility.DependencyInjection;
using Photon.Pun;
using UnityEngine;

namespace PacMan.Systems
{
    /*
     * A point controller to handle keeping score for the players in the scene 
     */
    public class PointController : MonoBehaviourPun
    {
        public static event Action<int, int> ScoreUpdated;
        
        private readonly Dictionary<int, int> _scores = new Dictionary<int, int>();

        private void Awake()
        {
            DependencyInjection.InjectAsType(this);
        }

        private void OnDestroy()
        {
            DependencyInjection.DeleteDependency<PointController>();
        }

        // Award points, limit to only the local player as we sync the points later on
        public void AwardPoints(Player player, int pointsToAward)
        {
            if (!_scores.ContainsKey(player.photonView.OwnerActorNr))
            {
                _scores.Add(player.photonView.OwnerActorNr, 0);
            }

            _scores[player.photonView.OwnerActorNr] += pointsToAward;
            
            photonView.RPC(nameof(UpdateScore), RpcTarget.All, player.photonView.OwnerActorNr, _scores[player.photonView.OwnerActorNr]);
        }

        // Update the score on both clients
        [PunRPC]
        public void UpdateScore(int actorNumber, int newScore)
        {
            ScoreUpdated?.Invoke(actorNumber, newScore);
        }

        // Get the score for a player
        private int GetScoreForPlayer(Player player)
        {
            int actorNumber = player.photonView.OwnerActorNr;
            return _scores.ContainsKey(actorNumber) ? _scores[actorNumber] : 0;
        }

        // Check for it the player has achieved a new local highscore
        public void CheckForHighscore(Player player)
        {
            int storedHighScore = PlayerPrefs.GetInt("HighScore", 0);
            int newHighScore = GetScoreForPlayer(player);

            if (newHighScore > storedHighScore)
            {
                PlayerPrefs.SetInt("HighScore", newHighScore);    
            }
        }
    }
}