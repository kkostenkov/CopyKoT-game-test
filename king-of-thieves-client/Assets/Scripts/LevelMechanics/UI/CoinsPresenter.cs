using MazeMechanics;

namespace LevelMechanics.UI
{
    public class CoinsPresenter
    {
        private ICoinsView view;
        private ICollectableManager coinKeeper;

        public CoinsPresenter(ICoinsView view, ICollectableManager coinKeeper)
        {
            this.view = view;
            this.coinKeeper = coinKeeper;
            this.coinKeeper.CoinBalanceUpdated += OnCoinBalanceUpdated;
        }

        private void OnCoinBalanceUpdated(int newBalance)
        {
            this.view.SetCoinCount(newBalance);
        }
    }
}