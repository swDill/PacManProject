using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PacMan.Systems
{
    /*
     * A static music controller to handle music tasks in our game, uses addressables to handle data management rather then the deprecated Resources.Load
     */
    public static class MusicController
    {
        private static AudioSource _audioSource;

        // Create the non destructible audio source
        private static void CreateMusicSource()
        {
            GameObject audioSourceObject = new GameObject("Music Source", typeof(AudioSource));
            Object.DontDestroyOnLoad(audioSourceObject);
            _audioSource = audioSourceObject.GetComponent<AudioSource>();
        }

        // Start loading the music track asynchronously, once complete then start playing the track 
        public static async void LoadAndPlayTrack(AssetReference assetReference, float volume = 1.0f, bool loop = false)
        {
            AsyncOperationHandle<AudioClip> handle = assetReference.LoadAssetAsync<AudioClip>();

            await handle.Task;

            AudioClip clip = handle.Result;

            if (clip == null)
            {
                Debug.LogError($"Asset { assetReference.SubObjectName } is not a valid AudioClip!");
                return;
            }
            
            PlayTrack(clip, volume, loop);
        }

        // Stop the current track, set the volume, then start playing the new one
        private static void PlayTrack(AudioClip audioClip, float volume = 1.0f, bool loop = false)
        {
            if (_audioSource == null)
            {
                // Create the audio source if we have not already
                CreateMusicSource();
            }

            // If we for some reason still dont have a audio source after attempting to creating one, then just exit early.
            if (_audioSource == null)
            {
                return;
            }
            
            _audioSource.Stop();
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.loop = loop;
            _audioSource.Play();
        }
    }
}