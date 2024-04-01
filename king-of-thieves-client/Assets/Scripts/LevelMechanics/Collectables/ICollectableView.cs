using System;
using UnityEngine;

namespace MazeMechanics
{
    public interface ICollectableView
    {
        void Place(Transform mazeCellTransform);
        event Action Touched;
        void Disable();
        void DrawChest();
        void DrawCoin();
    }
}