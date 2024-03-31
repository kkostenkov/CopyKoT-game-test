using System.Threading.Tasks;

namespace MazeMechanics
{
    internal class MazeBuilder
    {
        private DraftCellController cellController;
        private ICollectableManager collectableManager;

        public MazeBuilder(DraftCellController cellController, ICollectableManager collectableManager)
        {
            this.cellController = cellController;
            this.collectableManager = collectableManager;
        }
        
        public Task SpawnCells()
        {
            return this.cellController.SpawnCells();
        }

        public void Reset()
        {
            collectableManager.Reset();
        }
    }
}