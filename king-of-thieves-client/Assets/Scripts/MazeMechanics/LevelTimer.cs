using System;
using UnityEngine;

namespace MazeMechanics
{
    internal class LevelTimer : IUpdatable
    {
        public event Action Expired;

        private float levelTimeLimitSeconds = 5;
        private float secondsLeft = -1;
        private bool isActive;
    
        void IUpdatable.Update()
        {
            this.secondsLeft -= Time.deltaTime;
            if (this.secondsLeft <= 0) {
                StopAndReset();
                this.Expired?.Invoke();
            }
        }

        public void Start()
        {
            this.secondsLeft = this.levelTimeLimitSeconds;
            this.isActive = true;
        }

        private void StopAndReset()
        {
            this.isActive = false;
            this.secondsLeft = -1;
        }
    }
}