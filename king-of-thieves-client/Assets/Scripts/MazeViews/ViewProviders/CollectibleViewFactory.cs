using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectibleViewFactory : MonoBehaviour, ICollectibleViewFactory
    {
        // [SerializeField]
        // private CollectibleView collectibleViewPrefab;
    
        public ICollectibleView GetView()
        {
            throw new System.NotImplementedException();
        }
    }
}