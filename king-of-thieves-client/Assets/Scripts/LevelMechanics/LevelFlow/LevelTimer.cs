using System;
using LevelMechanics.Settings;
using UnityEngine;

namespace LevelMechanics
{
    public class LevelTimer : IUpdatable, ILevelTimeProvider
    {
        private readonly ILevelStateInfoProvider levelState;
        public event Action Expired;
        public event Action<int> SeсondsLeftUpdated;
        
        private float secondsLeft = -1;
        private bool isActive;
        private float lastTimeEventFired;
        private ITimeSettingsProvider settings;

        public LevelTimer(ILevelStateInfoProvider levelState, ITimeSettingsProvider settings)
        {
            this.settings = settings;
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

            TryFireSecondTickEvent();
        }

        private void Start()
        {
            this.secondsLeft = settings.LevelTimeLimitSeconds;
            this.SeсondsLeftUpdated?.Invoke((int)this.secondsLeft);
            this.isActive = true;
            Debug.Log("Level timer started");
        }

        private void StopAndReset()
        {
            this.isActive = false;
            this.secondsLeft = -1;
        }

        private void TryFireSecondTickEvent()
        {
            var hasSecondPassed = Math.Abs(this.lastTimeEventFired - this.secondsLeft) >= 1f;
            if (hasSecondPassed) {
                this.SeсondsLeftUpdated?.Invoke((int)this.secondsLeft);
                this.lastTimeEventFired = this.secondsLeft;
            }
        }
    }
}