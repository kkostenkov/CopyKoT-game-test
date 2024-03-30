using System.Threading.Tasks;
using MazeMechanics.Cells;

namespace MazeMechanics
{
    public interface IMazeCellViewFactory
    {
        Task<IMazeCellView> GetView(MazeCellModel model);
    }
}