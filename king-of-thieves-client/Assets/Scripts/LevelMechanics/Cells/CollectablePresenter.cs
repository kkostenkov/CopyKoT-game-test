
using System;

namespace MazeMechanics
{
    public class CollectablePresenter
    {
        private ICollectableView view;
        public CollectableModel Model { get; private set; }
        public event Action<CollectablePresenter, int> Collected;

        public CollectablePresenter(CollectableModel model)
        {
            this.Model = model;
        }

        public void SetView(ICollectableView collectableView)
        {
            this.view = collectableView;
            this.view.Touched += OnCollectableTouched;
            UpdateView();
        }

        public void UpdateView()
        {
            if (this.Model?.CoinValue > 0) {
                this.view.Enable();
            }
            else {
                this.view.Disable();
            }
        }

        private void OnCollectableTouched()
        {
            this.view.Disable();
            var coins = Model.CoinValue; 
            Model.CoinValue = 0;
            UpdateView();
            Collected?.Invoke(this, coins);    
        }
    }
}