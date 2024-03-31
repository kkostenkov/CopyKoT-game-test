using System.Threading.Tasks;
using LevelMechanics;

namespace MazeMechanics
{
    internal class MazeBuilder
    {
        private DraftCellController cellController;
        private ICollectableManager collectableManager;
        private PlayerPresenter player;

        public MazeBuilder(DraftCellController cellController, ICollectableManager collectableManager, PlayerPresenter player)
        {
            this.player = player;
            this.cellController = cellController;
            this.collectableManager = collectableManager;
        }
        
        public Task Build()
        {
            return this.cellController.SpawnCells();
        }

        public void Reset()
        {
            collectableManager.Reset();
            player.ResetPosition();
        }
    }
}