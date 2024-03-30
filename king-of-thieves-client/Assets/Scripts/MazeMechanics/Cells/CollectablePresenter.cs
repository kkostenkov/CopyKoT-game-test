using System.Threading.Tasks;

namespace MazeMechanics
{
    public class CollectablePresenter
    {
        private ICollectibleView view;

        public CollectablePresenter(ICollectibleViewFactory factory)
        {
            this.view = factory.GetView();
        }

        public Task Spawn()
        {
            throw new System.NotImplementedException();
        }
    }
}