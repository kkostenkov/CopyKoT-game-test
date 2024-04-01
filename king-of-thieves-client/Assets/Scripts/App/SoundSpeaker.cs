using UnityEngine;

namespace Audio
{
    public class SoundSpeaker : MonoBehaviour, ISfxPlayer
    {
        [SerializeField]
        private AudioSource audioSource;
    
        public void Play()
        {
            this.audioSource.Play();
        }
    }

    public interface ISfxPlayer
    {
        void Play();
    }
}