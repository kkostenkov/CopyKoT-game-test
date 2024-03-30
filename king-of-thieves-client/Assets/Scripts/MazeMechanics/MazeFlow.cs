using System.Threading.Tasks;
using UnityEngine;

namespace MazeMechanics
{
    public class MazeFlow : MonoBehaviour
    {
        private LevelTimer timer;
        private LevelStateDispatcher levelState;

        private async Task Start()
        {
            CacheDependencies();
            await CreateLevel();
            InitiateLevelTimer();
            StartLevel();
        }

        private Task CreateLevel()
        {
            var maze = DI.Game.Resolve<DraftCellController>();
            return maze.SpawnCells();
        }

        private void CacheDependencies()
        {
            this.timer = DI.Game.Resolve<LevelTimer>();
            this.levelState = DI.Game.Resolve<LevelStateDispatcher>();
        }

        private void StartLevel()
        {
            this.levelState.Set(LevelState.Action);
        }

        private void StopLevel()
        {
            this.levelState.Set(LevelState.TimeIsUp);
        }

        private void InitiateLevelTimer()
        {
            this.timer.Expired += OnLevelTimeEnded;
            this.timer.Start();
        }

        private void OnLevelTimeEnded()
        {
            this.timer.Expired -= OnLevelTimeEnded;
            StopLevel();
        }
    }
}