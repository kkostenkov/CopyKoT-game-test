using System;

namespace MazeMechanics
{
    public interface ICollectableManager
    {
        event Action<int> CoinBalanceUpdated; 
        void Reset();
    }
}