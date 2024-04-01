using System;
using System.Collections.Generic;
using LevelMechanics;
using MazeMechanics.Cells;
using MazeMechanics.Storage;

namespace MazeMechanics
{
    internal class CollectableManager : ICollectablePresenterFactory, ICollectableManager
    {
        public event Action<int> CoinBalanceUpdated;
        public int CoinBalance { get; private set; }

        public int CoinsBest { get; private set; }

        private readonly Dictionary<int, CollectablePresenter> presenters = new();
        private readonly CollectableRefresher refresher;
        private readonly ILevelStateInfoProvider levelState;
        private readonly IScoreStorage storage;
        private readonly ITreasureGenerator treasure;

        public CollectableManager(CollectableRefresher refresher, ILevelStateInfoProvider levelState, IScoreStorage storage,
            ITreasureGenerator treasure)
        {
            this.treasure = treasure;
            this.storage = storage;
            this.levelState = levelState;
            this.refresher = refresher;
            this.refresher.Refreshed += OnCollectableRefreshed;
            this.levelState.SessionEnded += OnSessionEnded;
            CoinsBest = storage.GetCoinsBest();
        }

        public CollectablePresenter GetPresenter(MazeCellModel model)
        {
            var presenter = new CollectablePresenter();
            presenter.SetModel(this.treasure.GenerateTreasure());
            presenter.Collected += OnCollected;
            this.presenters.Add(model.Id, presenter);
            return presenter;
        }

        public void Reset()
        {
            refresher.Clear();
            foreach (var presenterKvP in presenters) {
                var presenter = presenterKvP.Value;
                presenter.SetModel(treasure.GenerateTreasure());
                presenter.UpdateView();    
            }
            this.CoinBalance = 0;
            CoinBalanceUpdated?.Invoke(this.CoinBalance);
        }

        private void OnCollected(CollectablePresenter presenter)
        {
            var coins = this.treasure.GetCoinValue(presenter.Model, CoinBalance);
            Score(coins);
            this.treasure.Clear(presenter.Model);
            presenter.UpdateView();
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
            presenter.SetModel(this.treasure.GenerateTreasure());
            presenter.UpdateView();
        }
    }
}