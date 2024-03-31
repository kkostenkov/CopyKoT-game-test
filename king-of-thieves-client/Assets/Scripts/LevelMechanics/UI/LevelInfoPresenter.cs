using System;
using MazeMechanics;

namespace LevelMechanics.UI
{
    public class LevelInfoPresenter
    {
        public event Action SessionStartRequested;
        
        private ILevelInfoView view;
        private ILevelStateInfoChanger levelState;

        public LevelInfoPresenter(ILevelInfoView view, ILevelStateInfoChanger levelState)
        {
            this.levelState = levelState;
            this.levelState.MazeLoaded += OnMazeLoaded;
            this.levelState.SessionEnded += OnSessionEnded;
            this.levelState.SessionStarted += OnSessionStarted;
            this.view = view;
            view.PlayPressed += OnPlayPressed;
        }

        private void OnSessionStarted()
        {
            this.view.Hide();
        }

        private void OnSessionEnded()
        {
            this.view.Show();
        }

        private void OnMazeLoaded()
        {
            this.view.Show();
        }

        private void OnPlayPressed()
        {
            SessionStartRequested?.Invoke();
        }
    }
}