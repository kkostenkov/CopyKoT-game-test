using System;

namespace MazeMechanics
{
    public interface ICollectableManager
    {
        event Action<int> CoinBalanceUpdated;
        int CoinBalance { get; }
        void Reset();
    }
}