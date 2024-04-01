namespace MazeMechanics
{
    internal interface ITreasureGenerator
    {
        void TryAddTreasure(MazeCellModel model);
        CollectableModel GenerateTreasure();
        int GetCoinValue(CollectableModel model, int coinBalance);
        void Clear(CollectableModel model);
    }
}