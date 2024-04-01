using System;
using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class CollectableView : MonoBehaviour, ICollectableView
    {
        [SerializeField]
        private SpriteRenderer collectableSprite;

        [SerializeField]
        private Sprite coin;
        [SerializeField]
        private Sprite chest;
        
        public event Action Touched;
        
        public void Disable()
        {
            this.gameObject.SetActive(false);
        }

        public void DrawChest()
        {
            collectableSprite.sprite = this.chest;
            this.gameObject.SetActive(true);
        }

        public void DrawCoin()
        {
            collectableSprite.sprite = this.coin;
            this.gameObject.SetActive(true);
        }

        public void OnTriggerEnter2D(Collider2D col)
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