using System.Threading.Tasks;
using UnityEngine;

namespace MazeMechanics
{
    public class LevelGameFlow : MonoBehaviour
    {
        private LevelTimer timer;
        private LevelStateDispatcher levelState;

        private async void Start()
        {
            CacheDependencies();
            await CreateLevel();
            InitiateLevelTimer();
            StartLevel();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private Task CreateLevel()
        {
            var maze = DI.Game.Resolve<DraftCellController>();
            return maze.SpawnCells();
        }

        private void CacheDependencies()
        {
            this.timer = DI.Game.Resolve<LevelTimer>();
            this.timer.Expired += OnLevelTimeEnded;
            this.levelState = DI.Game.Resolve<LevelStateDispatcher>();
            this.levelState.LevelStateChanged += OnLevelStateChanged;
        }

        private void OnLevelStateChanged(LevelState arg1, LevelState arg2)
        {
            
        }

        private void StartLevel()
        {
            this.levelState.Set(LevelState.Action);
        }

        private void InitiateLevelTimer()
        {
            this.timer.Start();
        }

        private void OnLevelTimeEnded()
        {
            this.timer.Expired -= OnLevelTimeEnded;
            this.levelState.Set(LevelState.TimeIsUp);
        }

        private void Unsubscribe()
        {
            if (this.timer != null) {
                this.timer.Expired -= OnLevelTimeEnded;    
            }

            if (this.levelState != null) {
                this.levelState.LevelStateChanged -= OnLevelStateChanged;    
            }
        }
    }
}