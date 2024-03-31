using System;

namespace MazeMechanics
{
    public interface ILevelStateInfoProvider
    {
        event Action<LevelState, LevelState> LevelStateChanged;
        event Action NewLevelRequested;
        event Action MazeLoaded;
        event Action SessionStarted;
        event Action SessionEnded;
        LevelState CurrentState { get; }
    }
}