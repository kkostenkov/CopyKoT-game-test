using System;

namespace LevelMechanics.UI
{
    public interface ILevelInfoView
    {
        event Action PlayPressed;
        void Show();
        void Hide();
    }
}