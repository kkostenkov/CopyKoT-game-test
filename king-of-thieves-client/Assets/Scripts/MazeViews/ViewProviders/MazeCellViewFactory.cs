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

        private ICollectibleViewFactory collectibleViewFactory;

        public void Inject(ICollectibleViewFactory collectibleViewFactory)
        {
            this.collectibleViewFactory = collectibleViewFactory;
        }
    
        public IMazeCellView GetView(MazeCellModel model)
        {
            var mazeCell = Instantiate(this.viewPrefab, GetPositionForCellId(model.Id), Quaternion.identity, this.root);
            mazeCell.name = $"Cell_{model.Id}";
            if (model.IsPassable) {
                this.collectibleViewFactory = DI.Game.Resolve<ICollectibleViewFactory>();
                var collectible = this.collectibleViewFactory.GetView();
                collectible.Place(mazeCell.transform);
            }
            return mazeCell;
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