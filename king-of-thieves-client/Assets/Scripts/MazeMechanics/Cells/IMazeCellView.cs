using System.Threading.Tasks;

namespace MazeMechanics.Cells
{
    public interface IMazeCellView
    {
        Task DrawPassable();
        Task DrawImpassable();
        ICollectableView CollectableView { get; }
    }
}