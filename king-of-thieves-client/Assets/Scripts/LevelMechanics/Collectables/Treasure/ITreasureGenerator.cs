using MazeMechanics.Cells;

namespace MazeMechanics
{
    internal interface ITreasureGenerator
    {
        void TryAddTreasure(MazeCellModel model);
        CollectableModel GenerateTreasure();
        void Clear(CollectableModel model);
    }
}