#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace PacMan.UI
{
    /*
     * A very simple button operation to quit the game when pressed
     */
    public class QuitGameButtonOperation : ButtonOperation
    {
        protected override void OnClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}