using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectableViewFactory : MonoBehaviour, ICollectableViewFactory
    {
        [SerializeField]
        private CollectableView collectableViewPrefab;
    
        public ICollectableView GetView()
        {
            return GameObject.Instantiate(this.collectableViewPrefab);
        }
    }
}