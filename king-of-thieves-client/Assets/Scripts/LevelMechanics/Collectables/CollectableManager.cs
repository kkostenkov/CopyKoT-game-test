using System.Collections.Generic;
using UnityEngine;

namespace MazeMechanics
{
    internal class CollectableManager : ICollectablePresenterFactory
    {
        private readonly Dictionary<int, CollectablePresenter> presenters = new();
        private readonly CollectableTimeout delayer;

        public CollectableManager(CollectableTimeout delayer)
        {
            this.delayer = delayer;
            this.delayer.TimedOut += OnPresenterTimedOut;
        }

        public CollectablePresenter GetPresenter(MazeCellModel model)
        {
            var presenter = new CollectablePresenter(model.CollectableModel);
            presenter.Collected += OnCollected;
            this.presenters.Add(model.Id, presenter);
            return presenter;
        }

        private void OnCollected(CollectablePresenter presenter, int coins)
        {
            // score
            Debug.Log($"Collected {coins} coins");
            // schedule refresh
            this.delayer.Schedule(presenter);
        }

        private void OnPresenterTimedOut(CollectablePresenter presenter)
        {
            presenter.Model.CoinValue = 1;
            presenter.UpdateView();
        }
    }
}