using System;
using UnityEngine;

namespace MazeMechanics
{
    public class LevelTimer : IUpdatable
    {
        private readonly ILevelStateInfoProvider levelState;
        public event Action Expired;

        private float levelTimeLimitSeconds = 5;
        private float secondsLeft = -1;
        private bool isActive;

        public LevelTimer(ILevelStateInfoProvider levelState)
        {
            this.levelState = levelState;
            levelState.SessionStarted += OnSessionStarted;
        }

        private void OnSessionStarted()
        {
            Start();
        }
    
        void IUpdatable.Update()
        {
            if (!this.isActive) {
                return;
            }
            
            this.secondsLeft -= Time.deltaTime;
            if (this.secondsLeft <= 0) {
                StopAndReset();
                Debug.Log("Level timer expired");
                this.Expired?.Invoke();
            }
        }

        private void Start()
        {
            this.secondsLeft = this.levelTimeLimitSeconds;
            this.isActive = true;
            Debug.Log("Level timer started");
        }

        private void StopAndReset()
        {
            this.isActive = false;
            this.secondsLeft = -1;
        }
    }
}