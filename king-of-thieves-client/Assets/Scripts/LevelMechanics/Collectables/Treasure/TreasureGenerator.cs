using System;
using MazeMechanics.Cells;
using MazeMechanics.Random;
using UnityEngine;

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

        public int GetCoinValue(CollectableModel model, int coinBalance)
        {
            switch (model.Treasure) {
                case TreasureKind.None:
                    return 0;
                case TreasureKind.Coin:
                    return 1;
                case TreasureKind.Chest:
                    return Math.Min(1, coinBalance / 10);
                default:
                    Debug.LogError($"missing case for {model.Treasure}");
                    return 0;
            }
        }

        public void Clear(CollectableModel model)
        {
            model.Treasure = TreasureKind.None;
        }
    }
}