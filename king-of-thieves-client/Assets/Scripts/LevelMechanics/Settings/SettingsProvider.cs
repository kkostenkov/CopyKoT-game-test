using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            Addressables.LoadAssetsAsync<GameSettingsScriptable>("GameSettings", null).Completed += OnLoadDone;
        }

        private void OnLoadDone(AsyncOperationHandle<IList<GameSettingsScriptable>> handle)
        {
            var settings = handle.Result[0];
            ChestChancePercent = settings.ChestChancePercent;
            MinSpawnDelaySeconds = settings.MinSpawnDelaySeconds;
            MaxSpawnDelaySeconds = settings.MaxSpawnDelaySeconds;
            LevelTimeLimitSeconds = settings.LevelTimeLimitSeconds;
            Addressables.Release(handle);
        }
    }
}