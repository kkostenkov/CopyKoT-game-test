using System.Collections.Generic;
using System.Threading.Tasks;
using LevelMechanics;
using LevelMechanics.LevelMechanics.Cells;

namespace MazeMechanics
{
    internal class MazeBuilder
    {
        private CellController cellController;
        private ICollectableManager collectableManager;
        private PlayerPresenter player;

        public MazeBuilder(CellController cellController, ICollectableManager collectableManager, PlayerPresenter player)
        {
            this.player = player;
            this.cellController = cellController;
            this.collectableManager = collectableManager;
        }
        
        public Task Build()
        {
            var mazeModel = GenerateMazeModel();
            return this.cellController.SpawnCells(mazeModel);
        }

        public void Reset()
        {
            collectableManager.Reset();
            player.ResetPosition();
        }

        private MazeModel GenerateMazeModel()
        {
            var seed = new System.Random().Next();
            var model = new MazeModel() {
                Width = 11,
                Height = 7,
                EntryX = 1,
                EntryY = 1,
                Seed = seed,
            };
                
            var maze = MazeGenerator.BuildMaze(model.Width, model.Height, model.EntryX, model.EntryY, model.Seed);
            List<MazeCellModel> cellModels = new();
            for (int y = 0; y < maze.GetLength(1); y++) {
                for (int x = 0; x < maze.GetLength(0); x++) {
                    var isWall = maze[x, y];
                    var mazeCellModel = new MazeCellModel {
                        Id = x + (y * model.Width - 1),
                        X = x,
                        Y = y,
                        IsPassable = !isWall,
                        CouldContainCollectables = !isWall,
                    };
                    if (mazeCellModel.CouldContainCollectables) {
                        mazeCellModel.CollectableModel = new CollectableModel() {
                            CoinValue = 1,
                        };
                    }
                    
                    cellModels.Add(mazeCellModel);
                }
            }

            model.Cells = cellModels;
            return model;
        }
    }
}