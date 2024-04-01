using System;
using System.Collections.Generic;
using LevelMechanics;
using MazeMechanics.Storage;

namespace MazeMechanics
{
    internal class CollectableManager : ICollectablePresenterFactory, ICollectableManager
    {
        public const int DefaultCollectableValue = 1;
        
        public event Action<int> CoinBalanceUpdated;
        public int CoinBalance { get; private set; }

        public int CoinsBest { get; private set; }

        private readonly Dictionary<int, CollectablePresenter> presenters = new();
        private readonly CollectableRefresher refresher;
        private ILevelStateInfoProvider levelState;
        private IScoreStorage storage;

        public CollectableManager(CollectableRefresher refresher, ILevelStateInfoProvider levelState, IScoreStorage storage)
        {
            this.storage = storage;
            this.levelState = levelState;
            this.refresher = refresher;
            this.refresher.Refreshed += OnCollectableRefreshed;
            this.levelState.SessionEnded += OnSessionEnded;
            CoinsBest = storage.GetCoinsBest();
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
            this.CoinBalance = 0;
            CoinBalanceUpdated?.Invoke(this.CoinBalance);
        }

        private void OnCollected(CollectablePresenter presenter, int coins)
        {
            Score(coins);
            ScheduleRefresh(presenter);
        }

        private void OnSessionEnded()
        {
            if (CoinBalance <= CoinsBest) {
                return;
            }

            CoinsBest = CoinBalance;
            this.storage.SetCoinsBest(CoinsBest);
        }

        private void ScheduleRefresh(CollectablePresenter presenter)
        {
            this.refresher.Schedule(presenter);
        }

        private void Score(int coins)
        {
            this.CoinBalance += coins;
            CoinBalanceUpdated?.Invoke(this.CoinBalance);
        }

        private void OnCollectableRefreshed(CollectablePresenter presenter)
        {
            presenter.Model.CoinValue = DefaultCollectableValue;
            presenter.UpdateView();
        }
    }
}