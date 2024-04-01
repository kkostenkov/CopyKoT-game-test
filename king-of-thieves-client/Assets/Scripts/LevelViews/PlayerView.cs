using LevelMechanics;
using UnityEngine;

namespace Views
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        private Transform selfTransform;
        private Vector3 initialPozition;
        
        private void Awake()
        {
            selfTransform = this.transform;
            initialPozition = this.selfTransform.position;
        }

        public void ResetPosition()
        {
            this.selfTransform.position = initialPozition;
        }
    }
}