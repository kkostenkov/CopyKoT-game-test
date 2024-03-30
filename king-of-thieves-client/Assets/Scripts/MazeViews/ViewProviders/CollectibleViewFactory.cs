using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectibleViewFactory : MonoBehaviour, ICollectibleViewFactory
    {
        [SerializeField]
        private CollectableView collectibleViewPrefab;
    
        public ICollectibleView GetView()
        {
            return GameObject.Instantiate(collectibleViewPrefab);
        }
    }
}