using MazeMechanics.Cells;
using MazeMechanics.Random;

namespace MazeMechanics
{
    public class TreasureGenerator : ITreasureGenerator
    {
        private ITreasureGenerationSettingsProvider settings;
        private IRandomProvider random;

        public TreasureGenerator(ITreasureGenerationSettingsProvider settings, IRandomProvider random)
        {
            this.random = random;
            this.settings = settings;
        }
        
        public void TryAddTreasure(MazeCellModel cellModel)
        {
            if (!cellModel.CouldContainCollectables) {
                cellModel.CollectableModel = CreateEmpty();
                return;
            }

            cellModel.CollectableModel = CreateTreasure();
        }

        public CollectableModel GenerateTreasure()
        {
            return CreateTreasure();
        }

        public void Clear(CollectableModel model)
        {
            model.Treasure = TreasureKind.None;
        }

        private CollectableModel CreateEmpty()
        {
            return new CollectableModel() {
                Treasure = TreasureKind.None,
            };
        }

        private CollectableModel CreateTreasure()
        {
            int percent = this.random.RollPercent();
            var isChest = percent < settings.ChestChancePercent;
            if (isChest) {
                return CreateChest();
            }

            return CreateCoin();
        }

        private CollectableModel CreateCoin()
        {
            var model = new CollectableModel {
                Treasure = TreasureKind.Coin
            };
            return model;
        }

        private CollectableModel CreateChest()
        {
            var model = new CollectableModel {
                Treasure = TreasureKind.Chest
            };
            return model;
        }
    }
}