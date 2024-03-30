using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectableView : MonoBehaviour, ICollectibleView
    {
        public void OnTriggerStay2D(Collider2D col)
        {
            Debug.Log("InCollectable trigger");
            gameObject.SetActive(false);
        }

        public void Place(Transform parent)
        {
            this.transform.SetParent(parent);
            this.transform.localPosition = Vector3.zero;
        }
    }
}