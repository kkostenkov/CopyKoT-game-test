using System.Collections.Generic;

namespace MazeMechanics
{
    public class MazeModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int EntryX { get; set; }
        public int EntryY { get; set; }
        public int Seed { get; set; }
        public List<MazeCellModel> Cells { get; set; }
    }
}