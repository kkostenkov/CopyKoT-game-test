namespace MazeMechanics
{
    class SettingsProvider : ITreasureGenerationSettingsProvider, ICollectableSpawnSettingsProvider
    {
        public int ChestChancePercent { get; } = 10;
        public int MinSpawnDelaySeconds { get; } = 3;
        public int MaxSpawnDelaySeconds { get; } = 10;
    }
}