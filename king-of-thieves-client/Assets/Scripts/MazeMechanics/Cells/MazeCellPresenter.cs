using System.Threading.Tasks;
using MazeMechanics.Cells;

namespace MazeMechanics
{
    public class MazeCellPresenter
    {
        private MazeCellModel model;
        private IMazeCellView view;
        private readonly CollectablePresenter collectablePresenter;
        private readonly IMazeCellViewFactory factory;

        public MazeCellPresenter(IMazeCellViewFactory factory, CollectablePresenter collectablePresenter)
        {
            this.factory = factory;
            this.collectablePresenter = collectablePresenter;
        }

        public async Task InitializeAsync(MazeCellModel mazeCellModel)
        {
            this.model = mazeCellModel;
            var viewSpawnTask = factory.GetView(mazeCellModel);
            this.view = await viewSpawnTask;
            if (this.model.IsPassable) {
                collectablePresenter.Initialize(this.view.CollectableView);
            }
        }
    }
}