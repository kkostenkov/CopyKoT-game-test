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
            var mazeCell = Instantiate(this.viewPrefab, this.root);
            mazeCell.name = $"Cell_{model.Id}";
            if (model.IsPassable) {
                this.collectibleViewFactory = DI.Game.Resolve<ICollectibleViewFactory>();
                var collectible = this.collectibleViewFactory.GetView();
                collectible.Place(mazeCell.transform);
            }
            return mazeCell;
        }
    }
}