using UnityEngine;

namespace Audio
{
    public class SoundSpeaker : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
    
        public void Play()
        {
            this.audioSource.Play();
        }
    }
}