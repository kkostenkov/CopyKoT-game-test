using System;

namespace LevelMechanics.UI
{
    public interface ILevelInfoView
    {
        event Action PlayPressed;
        event Action ReplayPressed;
        void ShowWelcome(int maxScore);
        public void ShowGameOver(int score, int maxScore);
        void Hide();
    }
}