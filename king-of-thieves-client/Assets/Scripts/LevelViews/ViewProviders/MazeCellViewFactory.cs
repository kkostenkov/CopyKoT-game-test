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
            var position = GetPositionForCoords(model.X, model.Y);
            var mazeCell = Instantiate(this.viewPrefab, position, Quaternion.identity, this.root);
            mazeCell.name = $"Cell_{model.Id}";
            
            var initTask = model.IsPassable ? mazeCell.DrawPassable() : mazeCell.DrawImpassable();
            await initTask;
            if (model.CouldContainCollectables) {
                await AddCollectableView(mazeCell);
            }
            return mazeCell;
        }

        private Task AddCollectableView(MazeCellView mazeCellView)
        {
            var collectible = this.collectableViewFactory.GetView();
            collectible.Place(mazeCellView.transform);
            mazeCellView.CollectableView = collectible;
            return Task.CompletedTask;
        }

        private Vector3 GetPositionForCoords(int x, int y)
        {
            var spriteWidth = 4;
            return new Vector3(x, y, 0) * spriteWidth;
        }
    }
}