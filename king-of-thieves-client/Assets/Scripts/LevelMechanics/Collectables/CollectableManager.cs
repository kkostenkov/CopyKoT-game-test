using System.Collections.Generic;

namespace MazeMechanics
{
    internal class CollectableManager : ICollectablePresenterFactory, ICollectableManager
    {
        public const int DefaultCollectableValue = 1;
        
        private readonly Dictionary<int, CollectablePresenter> presenters = new();
        private readonly CollectableRefresher refresher;

        private int collectedCoins = 0;

        public CollectableManager(CollectableRefresher refresher)
        {
            this.refresher = refresher;
            this.refresher.Refreshed += OnCollectableRefreshed;
        }

        public CollectablePresenter GetPresenter(MazeCellModel model)
        {
            var presenter = new CollectablePresenter(model.CollectableModel);
            presenter.Collected += OnCollected;
            this.presenters.Add(model.Id, presenter);
            return presenter;
        }

        public void Reset()
        {
            refresher.Clear();
            foreach (var presenterKvP in presenters) {
                var presenter = presenterKvP.Value;
                presenter.Model.CoinValue = DefaultCollectableValue;
                presenter.UpdateView();    
            }
        }

        private void OnCollected(CollectablePresenter presenter, int coins)
        {
            Score(coins);
            ScheduleRefresh(presenter);
        }

        private void ScheduleRefresh(CollectablePresenter presenter)
        {
            this.refresher.Schedule(presenter);
        }

        private void Score(int coins)
        {
            this.collectedCoins += coins;
        }

        private void OnCollectableRefreshed(CollectablePresenter presenter)
        {
            presenter.Model.CoinValue = DefaultCollectableValue;
            presenter.UpdateView();
        }
    }
}