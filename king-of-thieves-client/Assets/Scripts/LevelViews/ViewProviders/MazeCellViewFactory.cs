using System.Threading.Tasks;
using MazeMechanics;
using MazeMechanics.Cells;
using UnityEngine;

namespace Views
{
    public class MazeCellViewFactory : MonoBehaviour, IMazeCellViewFactory
    {
        [SerializeField]
        private MazeCellView viewPrefab;

        [SerializeField]
        private Transform root;

        private ICollectableViewFactory collectableViewFactory;

        public void Inject(ICollectableViewFactory collectableViewFactory)
        {
            this.collectableViewFactory = collectableViewFactory;
        }
    
        public async Task<IMazeCellView> GetView(MazeCellModel model)
        {
            var mazeCell = Instantiate(this.viewPrefab, GetPositionForCellId(model.Id), Quaternion.identity, this.root);
            mazeCell.name = $"Cell_{model.Id}";

            Task initTask;
            if (model.IsPassable) {
                initTask = Task.WhenAll(mazeCell.DrawPassable(), AddCollectableView(mazeCell));
            }
            else {
                initTask = mazeCell.DrawImpassable();
            }

            await initTask;
            return mazeCell;
        }

        private Task AddCollectableView(MazeCellView mazeCellView)
        {
            var collectible = this.collectableViewFactory.GetView();
            collectible.Place(mazeCellView.transform);
            mazeCellView.CollectableView = collectible;
            return Task.CompletedTask;
        }

        private Vector3 GetPositionForCellId(int id)
        {
            var mapWidth = 7;
            var row = id % mapWidth;
            var column = id / mapWidth;
            var spriteWidth = 4;
            return new Vector3(row, column, 0) * spriteWidth;
        }
    }
}