using MazeMechanics.Cells;

namespace MazeMechanics
{
    public interface ICollectablePresenterFactory
    {
        CollectablePresenter GetPresenter(MazeCellModel model);
    }
}