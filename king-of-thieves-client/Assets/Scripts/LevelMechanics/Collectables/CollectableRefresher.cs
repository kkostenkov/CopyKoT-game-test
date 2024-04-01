using System;
using System.Collections.Generic;
using MazeMechanics.Random;

namespace MazeMechanics
{
    internal class CollectableRefresher : IUpdatable
    {
        public event Action<CollectablePresenter> Refreshed;
        
        private readonly List<WaitingPresenter> waitingPresenters = new();
        private IRandomProvider rand;
        private ICollectableSpawnSettingsProvider settings;

        public CollectableRefresher(IRandomProvider random, ICollectableSpawnSettingsProvider settings)
        {
            this.settings = settings;
            this.rand = random;
        }

        public void Schedule(CollectablePresenter presenter)
        {
            var delay = this.rand.Next(settings.MinSpawnDelaySeconds, this.settings.MaxSpawnDelaySeconds + 1);
            var entry = new WaitingPresenter(DateTime.UtcNow + TimeSpan.FromSeconds(delay), presenter);
            this.waitingPresenters.Add(entry);
        }

        void IUpdatable.Update()
        {
            for (var i = this.waitingPresenters.Count - 1; i >= 0; i--) {
                var entry = this.waitingPresenters[i];
                if (DateTime.UtcNow >= entry.DueDate) {
                    waitingPresenters.RemoveAt(i);
                    this.Refreshed?.Invoke(entry.Presenter);
                }
            }
        }

        public void Clear()
        {
            waitingPresenters.Clear();
        }
    }
    
    internal struct WaitingPresenter
    {
        public DateTime DueDate;
        public readonly CollectablePresenter Presenter;

        public WaitingPresenter(DateTime dueDate, CollectablePresenter presenter)
        {
            this.Presenter = presenter;
            this.DueDate = dueDate;
        }
    }
}