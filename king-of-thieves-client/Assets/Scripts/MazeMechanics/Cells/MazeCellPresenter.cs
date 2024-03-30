using System.Threading.Tasks;
using MazeMechanics.Cells;

namespace MazeMechanics
{
    public class MazeCellPresenter
    {
        private MazeCellModel model;
        private IMazeCellView view;
        private CollectablePresenter collectablePresenter;
        private IMazeCellViewFactory factory;

        public MazeCellPresenter(IMazeCellViewFactory factory, CollectablePresenter collectablePresenter)
        {
            this.factory = factory;
            this.collectablePresenter = collectablePresenter;
        }

        public Task Initialize(MazeCellModel mazeCellModel)
        {
            this.view = factory.GetView();
            this.model = mazeCellModel;
            Task initTask;
            if (this.model.IsPassable) {
                initTask = Task.WhenAll(this.view.DrawPassable(), SpawnCollectable());
            }
            else {
                initTask = this.view.DrawImpassable();
            }

            return initTask;
        }

        private Task SpawnCollectable()
        {
            return Task.CompletedTask;
            //return this.collectablePresenter.Spawn();
        }
    }
}