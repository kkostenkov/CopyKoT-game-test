using UnityEngine;

namespace MazeMechanics
{
    public interface ICollectibleView
    {
        void Place(Transform mazeCellTransform);
    }
}