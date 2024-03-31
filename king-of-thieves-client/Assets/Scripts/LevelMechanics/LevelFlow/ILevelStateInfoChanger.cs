namespace LevelMechanics
{
    public interface ILevelStateInfoChanger : ILevelStateInfoProvider
    {
        void Set(LevelState newState);
    }
}