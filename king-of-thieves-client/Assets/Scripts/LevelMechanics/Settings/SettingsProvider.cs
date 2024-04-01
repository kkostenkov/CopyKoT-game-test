using UnityEngine.AddressableAssets;

namespace LevelMechanics.Settings
{
    class SettingsProvider : ITreasureGenerationSettingsProvider, ICollectableSpawnSettingsProvider, ITimeSettingsProvider
    {
        public int ChestChancePercent { get; private set; }
        public int MinSpawnDelaySeconds { get; private set; }
        public int MaxSpawnDelaySeconds { get; private set; }
        public int LevelTimeLimitSeconds { get; private set; }
        
        public SettingsProvider()
        {
            var handle = Addressables.LoadAssetAsync<GameSettingsScriptable>("GameSettings");
            handle.WaitForCompletion();
            var settings = handle.Result;
            ChestChancePercent = settings.ChestChancePercent;
            MinSpawnDelaySeconds = settings.MinSpawnDelaySeconds;
            MaxSpawnDelaySeconds = settings.MaxSpawnDelaySeconds;
            LevelTimeLimitSeconds = settings.LevelTimeLimitSeconds;
            Addressables.Release(handle);
            // There is no reason now to play with async loading. 
        }
    }
}