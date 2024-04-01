using System;
using Audio;
using UnityEngine;

namespace MazeMechanics
{
    public class CollectablePresenter
    {
        private readonly ISfxPlayer sfxPlayer;
        private ICollectableView view;
        public CollectableModel Model { get; private set; }
        public event Action<CollectablePresenter> Collected;

        public CollectablePresenter(ISfxPlayer sfxPlayer)
        {
            this.sfxPlayer = sfxPlayer;
        }

        public void SetModel(CollectableModel newModel)
        {
            this.Model = newModel;
        }

        public void SetView(ICollectableView collectableView)
        {
            this.view = collectableView;
            this.view.Touched += OnCollectableTouched;
            UpdateView();
        }

        public void UpdateView()
        {
            if (Model == null || Model.Treasure == TreasureKind.None) {
                this.view.Disable();
                return;
            }
            
            switch (Model.Treasure) {
                case TreasureKind.Coin:
                    this.view.DrawCoin();
                    break;
                case TreasureKind.Chest:
                    this.view.DrawChest();
                    break;
                default:
                    this.view.Disable();
                    Debug.LogError($"unimplemented case for {Model.Treasure}");
                    break;
            }
        }

        private void OnCollectableTouched()
        {
            sfxPlayer.Play();
            Collected?.Invoke(this);    
        }
    }
}