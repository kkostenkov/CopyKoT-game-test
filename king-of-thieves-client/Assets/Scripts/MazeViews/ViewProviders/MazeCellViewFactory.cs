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
    
        public IMazeCellView GetView()
        {
            var instance = Instantiate(this.viewPrefab, this.root);
            return instance;
        }
    }
}