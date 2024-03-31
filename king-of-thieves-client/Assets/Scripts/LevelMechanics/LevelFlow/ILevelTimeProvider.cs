using System;

namespace LevelMechanics
{
    public interface ILevelTimeProvider
    {
        event Action Expired;
        event Action<int> SeсondsLeftUpdated;
    }
}