using System.Collections.Generic;

namespace MazeMechanics
{
    internal class CollectableManager : ICollectablePresenterFactory
    {
        private readonly Dictionary<int, CollectablePresenter> presenters = new();
        public CollectablePresenter GetPresenter(MazeCellModel model)
        {
            var presenter = new CollectablePresenter(model.CollectableModel);
            presenter.Collected += OnCollected;
            this.presenters.Add(model.Id, presenter);
            return presenter;
        }

        private void OnCollected(CollectablePresenter presenter)
        {
            
        }
    }
}