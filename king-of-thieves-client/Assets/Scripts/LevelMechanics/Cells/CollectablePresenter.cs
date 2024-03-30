
using System;

namespace MazeMechanics
{
    public class CollectablePresenter
    {
        private ICollectableView view;
        public CollectableModel Model { get; private set; }
        public event Action<CollectablePresenter> Collected;

        public CollectablePresenter(CollectableModel model)
        {
            this.Model = model;
        }

        public void SetView(ICollectableView collectableView)
        {
            this.view = collectableView;
            this.view.Touched += OnCollectableTouched;
            if (this.Model == null) {
                this.view.Disable();
            }
        }

        private void OnCollectableTouched()
        {
            this.view.Disable();
            Collected?.Invoke(this);    
        }
    }
}