using System;
using System.Text;

namespace LevelMechanics.UI
{
    public class LevelTimePresenter
    {
        private ILevelTimeView view;
        private ILevelTimeProvider levelTime;

        public LevelTimePresenter(ILevelTimeView view, ILevelTimeProvider levelTime)
        {
            this.view = view;
            this.levelTime = levelTime;
            this.levelTime.SeсondsLeftUpdated += OnSecondTicked;
        }

        private void OnSecondTicked(int secondsLeft)
        {
            secondsLeft = Math.Max(0, secondsLeft);
            var timeText = $"{secondsLeft / 60:d2}:{secondsLeft % 60:d2}";
            this.view.SetTimeText(timeText);
        }
    }
}