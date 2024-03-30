using System.Threading.Tasks;
using UnityEngine;

namespace MazeMechanics.Cells
{
    public class MazeCellView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private BoxCollider2D cellCollider;

        public Task DrawPassable()
        {
            spriteRenderer.enabled = false;
            cellCollider.enabled = false;
            return Task.CompletedTask;
        }

        public Task DrawImpassable()
        {
            spriteRenderer.enabled = true;
            cellCollider.enabled = true;
            return Task.CompletedTask;
        }
    }
}
