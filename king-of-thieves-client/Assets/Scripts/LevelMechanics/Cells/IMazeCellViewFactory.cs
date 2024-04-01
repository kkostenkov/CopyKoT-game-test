using System.Threading.Tasks;
using MazeMechanics.Cells;

namespace MazeMechanics.Cells
{
    public interface IMazeCellViewFactory
    {
        Task<IMazeCellView> GetView(MazeCellModel model);
    }
}