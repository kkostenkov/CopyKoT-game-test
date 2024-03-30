using System;
using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectableView : MonoBehaviour, ICollectableView
    {
        public event Action Touched;
        public void Disable()
        {
            this.gameObject.SetActive(false);
        }

        public void Enable()
        {
            this.gameObject.SetActive(true);
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            Touched?.Invoke();
        }

        public void Place(Transform parent)
        {
            this.transform.SetParent(parent);
            this.transform.localPosition = Vector3.zero;
        }
    }
}