using PacMan.Systems;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PacMan
{
    /*
     * Simple class to start music when the object is awoken.
     */
    public class MusicStarter : MonoBehaviour
    {
        [SerializeField] private AssetReference _audioClipReference;
        [SerializeField, Range(0f, 1f)] private float _volume;
        [SerializeField] private bool _loop;

        private void Awake()
        {
            MusicController.LoadAndPlayTrack(_audioClipReference, _volume, _loop);
        }
    }
}