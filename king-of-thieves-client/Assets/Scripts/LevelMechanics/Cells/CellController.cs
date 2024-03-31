using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazeMechanics
{
    public class CellController
    {
        private List<MazeCellPresenter> cellPresenters;
        private MazeModel model;

        public Task SpawnCells(MazeModel model)
        {
            this.model = model;
            return InitMazeCells(model);
        }

        private Task InitMazeCells(MazeModel model)
        {
            var tasks = new List<Task>();
            foreach (var cellModel in model.Cells) {
                var task = CreateCellAsync(cellModel);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }

        private static Task CreateCellAsync(MazeCellModel model)
        {
            var presenter = DI.Game.Resolve<MazeCellPresenter>();
            var initCallTask = presenter.InitializeAsync(model);
            return initCallTask;
        }
    }
}