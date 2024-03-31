namespace MazeMechanics
{
    public interface ILevelStateInfoChanger : ILevelStateInfoProvider
    {
        void Set(LevelState newState);
    }
}