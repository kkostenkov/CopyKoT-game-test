
namespace MazeMechanics
{
    public class CollectablePresenter
    {
        private ICollectableView view;

        public void Initialize(ICollectableView collectableView)
        {
            this.view = collectableView;
            this.view.Touched += OnCollectableTouched;
        }

        private void OnCollectableTouched()
        {
            this.view.Disable();
        }
    }
}