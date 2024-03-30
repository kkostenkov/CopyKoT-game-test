using MazeMechanics.Cells;

namespace MazeMechanics
{
    public interface IMazeCellViewFactory
    {
        IMazeCellView GetView();
    }
}