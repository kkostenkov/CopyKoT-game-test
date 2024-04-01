using UnityEngine;

namespace MazeMechanics.Storage
{
    public class PlayerPrefsStorage : IScoreStorage
    {
        public const string CoinsBestKey = "coins_best";
        public int GetCoinsBest()
        {
            return PlayerPrefs.GetInt(CoinsBestKey);
        }

        public void SetCoinsBest(int coinsBest)
        {
            PlayerPrefs.SetInt(CoinsBestKey, coinsBest);
        }
    }
}