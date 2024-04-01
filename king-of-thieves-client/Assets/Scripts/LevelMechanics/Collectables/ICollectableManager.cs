using System;

namespace MazeMechanics
{
    public interface ICollectableManager
    {
        event Action<int> CoinBalanceUpdated;
        int CoinBalance { get; }
        int CoinsBest { get; }
        void Reset();
    }
}