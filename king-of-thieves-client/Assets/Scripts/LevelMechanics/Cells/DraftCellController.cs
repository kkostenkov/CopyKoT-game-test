using System.Collections.Generic;
using System.Threading.Tasks;
using Random = System.Random;

namespace MazeMechanics
{
    public class DraftCellController
    {
        private int seed = 48;
        private int cellsToSpawn = 28;
        private List<MazeCellPresenter> cellPresenters;

        public Task SpawnCells()
        {
            return InitMazeCellsAsync();
        }

        private async Task InitMazeCellsAsync()
        {
            var rand = new Random(this.seed);
            var mazeCellModel = new MazeCellModel {
                Id = 0,
                IsPassable = true
            };
            var presenter = DI.Game.Resolve<MazeCellPresenter>();
            var initCallTask = presenter.InitializeAsync(mazeCellModel);
            await initCallTask;
            for (var index = 1; index < cellsToSpawn; index++) {
                initCallTask = CreateCellAsync(index, rand);
                await initCallTask;
            }
        }

        private static Task CreateCellAsync(int index, Random rand)
        {
            var mazeCellModel = new MazeCellModel {
                Id = index,
                IsPassable = rand.Next() % 2 == 0
            };
            var presenter = DI.Game.Resolve<MazeCellPresenter>();
            var initCallTask = presenter.InitializeAsync(mazeCellModel);
            return initCallTask;
        }
    }
}