using System.Collections.Generic;
using System.Threading.Tasks;
using MazeMechanics.Cells;
using UnityEngine;

namespace MazeMechanics
{
    public class DraftCellController : MonoBehaviour
    {
        [SerializeField]
        private List<MazeCellView> cells;
        [SerializeField]
        private int seed = 48;
        
        private async Task Awake()
        {
            await DrawMazeCells();
        }

        private async Task DrawMazeCells()
        {
            var rand = new System.Random(this.seed);
            var drawCellTask = this.cells[0].DrawPassable();
            await drawCellTask;
            for (var index = 1; index < this.cells.Count; index++) {
                var cell = this.cells[index]; 
                drawCellTask = rand.Next() % 2 == 0 ? cell.DrawPassable() : cell.DrawImpassable();
                await drawCellTask;
            }
        }
    }
}
