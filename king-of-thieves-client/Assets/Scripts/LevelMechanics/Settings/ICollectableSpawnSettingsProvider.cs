namespace LevelMechanics.Settings
{
    internal interface ICollectableSpawnSettingsProvider
    {
        int MinSpawnDelaySeconds { get; }
        int MaxSpawnDelaySeconds { get; }
    }
}