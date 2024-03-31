namespace LevelMechanics
{
    public class PlayerPresenter
    {
        private IPlayerView view;

        public PlayerPresenter(IPlayerView playerView)
        {
            this.view = playerView;
        }

        public void ResetPosition()
        {
            view.ResetPosition();
        }
    }
}