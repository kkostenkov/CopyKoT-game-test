namespace MazeMechanics
{
    internal interface ICollectableSpawnSettingsProvider
    {
        int MinSpawnDelaySeconds { get; }
        int MaxSpawnDelaySeconds { get; }
    }
}