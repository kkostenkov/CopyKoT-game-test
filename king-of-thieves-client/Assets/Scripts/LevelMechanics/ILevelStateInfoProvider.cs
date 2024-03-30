using System;

namespace MazeMechanics
{
    public interface ILevelStateInfoProvider
    {
        event Action<LevelState, LevelState> LevelStateChanged;
        LevelState CurrentState { get; }
    }
}