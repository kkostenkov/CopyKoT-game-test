using System;

namespace MazeMechanics
{
    public class LevelStateDispatcher : ILevelStateInfoProvider
    {
        public event Action<LevelState, LevelState> LevelStateChanged;
        public LevelState CurrentState { get; private set; } = LevelState.Unknown;
    
        public void Set(LevelState newState)
        {
            if (CurrentState == newState) {
                return;
            }

            var oldState = CurrentState;
            CurrentState = newState;
        
            this.LevelStateChanged?.Invoke(oldState, newState);
        }
    }
}