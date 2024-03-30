using System;
using System.Collections.Generic;

namespace MazeMechanics
{
    internal class CollectableTimeout : IUpdatable
    {
        public event Action<CollectablePresenter> TimedOut;
        
        private readonly List<WaitingPresenter> waitingPresenters = new();
        private readonly Random rand = new Random();
        private int minDelaySeconds = 2;
        private int maxDelaySeconds = 10;

        public void Schedule(CollectablePresenter presenter)
        {
            var delay = this.rand.Next(minDelaySeconds, this.maxDelaySeconds + 1);
            var entry = new WaitingPresenter(DateTime.UtcNow + TimeSpan.FromSeconds(delay), presenter);
            this.waitingPresenters.Add(entry);
        }

        void IUpdatable.Update()
        {
            for (var i = this.waitingPresenters.Count - 1; i >= 0; i--) {
                var entry = this.waitingPresenters[i];
                if (DateTime.UtcNow >= entry.DueDate) {
                    waitingPresenters.RemoveAt(i);
                    TimedOut?.Invoke(entry.Presenter);
                }
            }
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