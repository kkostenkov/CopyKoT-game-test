using System.Threading.Tasks;
using MazeMechanics;
using MazeMechanics.Cells;
using UnityEngine;

namespace Views
{
    public class MazeCellView : MonoBehaviour, IMazeCellView
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private BoxCollider2D cellCollider;

        public ICollectableView CollectableView { get; set; }

        public Task DrawPassable()
        {
            this.spriteRenderer.enabled = true;
            this.cellCollider.enabled = false;
            return Task.CompletedTask;
        }

        public Task DrawImpassable()
        {
            this.spriteRenderer.enabled = false;
            this.cellCollider.enabled = true;
            return Task.CompletedTask;
        }
    }
}