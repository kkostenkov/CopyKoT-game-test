using System.Threading.Tasks;
using LevelMechanics.UI;
using UnityEngine;

namespace MazeMechanics
{
    public class LevelGameFlow : MonoBehaviour
    {
        private LevelTimer timer;
        private ILevelStateInfoChanger levelState;
        private LevelInfoPresenter levelInfoPresenter;
        private MazeBuilder mazeBuilder;

        private async void Start()
        {
            CacheDependencies();
            await CreateLevel();
            await InitializeUI();
            SetMazeLoadedState();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private Task CreateLevel()
        {
            return this.mazeBuilder.Build();
        }

        private Task InitializeUI()
        {
            DI.Game.Resolve<LevelInfoPresenter>();
            return Task.CompletedTask;
        }

        private void CacheDependencies()
        {
            this.mazeBuilder = DI.Game.Resolve<MazeBuilder>();
            this.timer = DI.Game.Resolve<LevelTimer>();
            this.levelState = DI.Game.Resolve<ILevelStateInfoChanger>();
            this.levelInfoPresenter = DI.Game.Resolve<LevelInfoPresenter>();
            Subscribe();
        }
        
        private void SetMazeLoadedState()
        {
            this.levelState.Set(LevelState.MazeLoaded);
        }

        private void OnLevelTimeEnded()
        {
            this.levelState.Set(LevelState.SessionEnded);
        }

        private void OnSessonStartRequested()
        {
            this.mazeBuilder.Reset();
            this.levelState.Set(LevelState.SessionStarted);
        }

        private void Subscribe()
        {
            this.timer.Expired += OnLevelTimeEnded;
            this.levelInfoPresenter.SessionStartRequested += OnSessonStartRequested;
        }

        private void Unsubscribe()
        {
            this.timer.Expired -= OnLevelTimeEnded;
            this.levelInfoPresenter.SessionStartRequested -= OnSessonStartRequested;
        }
    }
}