using System;
using MazeMechanics;

namespace LevelMechanics.UI
{
    public class LevelInfoPresenter
    {
        public event Action SessionStartRequested;
        public event Action NewLevelRequested;
        
        private ILevelInfoView view;
        private ILevelStateInfoProvider levelState;
        private ICollectableManager collectableManager;

        public LevelInfoPresenter(ILevelInfoView view, ILevelStateInfoProvider levelState, ICollectableManager collectableManager)
        {
            this.collectableManager = collectableManager;
            this.levelState = levelState;
            this.levelState.MazeLoaded += OnMazeLoaded;
            this.levelState.SessionEnded += OnSessionEnded;
            this.levelState.SessionStarted += OnSessionStarted;
            this.view = view;
            view.PlayPressed += OnPlayPressed;
            view.ReplayPressed += OnReplayPressed;
        }

        private void OnSessionStarted()
        {
            this.view.Hide();
        }

        private void OnSessionEnded()
        {
            var sessionScore = collectableManager.CoinBalance;
            var maxScore = collectableManager.CoinsBest;
            this.view.ShowGameOver(sessionScore, maxScore);
        }

        private void OnMazeLoaded()
        {
            var maxScore = collectableManager.CoinsBest;
            this.view.ShowWelcome(maxScore);
        }

        private void OnPlayPressed()
        {
            if (levelState.CurrentState == LevelState.MazeLoaded) {
                SessionStartRequested?.Invoke();
                return;
            }
            
            NewLevelRequested?.Invoke();
        }

        private void OnReplayPressed()
        {
            SessionStartRequested?.Invoke();
        }
    }
}