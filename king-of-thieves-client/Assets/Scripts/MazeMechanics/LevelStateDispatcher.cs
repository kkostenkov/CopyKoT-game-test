using System;

internal class LevelStateDispatcher : ILevelStateInfoProvider
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
        
        LevelStateChanged?.Invoke(oldState, newState);
    }
}

public interface ILevelStateInfoProvider
{
    event Action<LevelState, LevelState> LevelStateChanged;
    LevelState CurrentState { get; }
}