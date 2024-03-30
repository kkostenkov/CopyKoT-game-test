using System.Threading.Tasks;
using MazeMechanics.Cells;

namespace MazeMechanics
{
    public class MazeCellPresenter
    {
        private MazeCellModel model;
        private IMazeCellView view;
        private readonly ICollectablePresenterFactory collectablePresenterFactory;
        private readonly IMazeCellViewFactory factory;

        public MazeCellPresenter(IMazeCellViewFactory factory, ICollectablePresenterFactory collectablePresenterFactory)
        {
            this.factory = factory;
            this.collectablePresenterFactory = collectablePresenterFactory;
        }

        public async Task InitializeAsync(MazeCellModel mazeCellModel)
        {
            this.model = mazeCellModel;
            var viewSpawnTask = factory.GetView(mazeCellModel);
            this.view = await viewSpawnTask;
            if (this.model.CouldContainCollectables) {
                var collectablePresenter = collectablePresenterFactory.GetPresenter(mazeCellModel);
                collectablePresenter.SetView(this.view.CollectableView);    
            }
        }
    }
}