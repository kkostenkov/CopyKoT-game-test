using System;
using UnityEngine;

namespace MazeMechanics
{
    public class LevelStateDispatcher : ILevelStateInfoChanger
    {
        public event Action<LevelState, LevelState> LevelStateChanged;
        public event Action NewLevelRequested;
        public event Action MazeLoaded;
        public event Action SessionStarted;
        public event Action SessionEnded;
        public LevelState CurrentState { get; private set; } = LevelState.Unknown;
    
        public void Set(LevelState newState)
        {
            if (CurrentState == newState) {
                return;
            }

            var oldState = CurrentState;
            CurrentState = newState;
            Debug.Log($"Changed level state {oldState} -> {newState}");
            
            FireEvents(newState, oldState);
        }

        private void FireEvents(LevelState newState, LevelState oldState)
        {
            this.LevelStateChanged?.Invoke(oldState, newState);
            switch (newState) {
                case LevelState.NewLevelRequested:
                    this.NewLevelRequested?.Invoke();
                    break;
                case LevelState.MazeLoaded:
                    this.MazeLoaded?.Invoke();
                    break;
                case LevelState.SessionStarted:
                    this.SessionStarted?.Invoke();
                    break;
                case LevelState.SessionEnded:
                    this.SessionEnded?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}