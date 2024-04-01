namespace MazeMechanics.Cells
{
    public class MazeCellModel
    {
        public int Id;
        public bool IsPassable;
        public bool CouldContainCollectables;

        public CollectableModel CollectableModel;
        public int X { get; set; }
        public int Y { get; set; }
    }
}