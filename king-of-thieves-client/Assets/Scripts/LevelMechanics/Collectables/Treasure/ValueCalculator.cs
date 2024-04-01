using System;
using UnityEngine;

namespace MazeMechanics
{
    public static class ValueCalculator
    {
        public static int GetCoinValue(CollectableModel model, int coinBalance)
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
    }
}