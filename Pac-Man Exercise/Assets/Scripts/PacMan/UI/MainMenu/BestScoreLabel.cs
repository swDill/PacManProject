using TMPro;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * Simple class to retrieve the players highscore from the player prefs
     */
    public class BestScoreLabel : MonoBehaviour
    {
        private TextMeshProUGUI _textLabel;
        
        private void Awake()
        {
            _textLabel = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _textLabel.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }
}