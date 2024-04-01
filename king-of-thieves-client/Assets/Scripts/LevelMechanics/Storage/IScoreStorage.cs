namespace MazeMechanics.Storage
{
    internal interface IScoreStorage
    {
        int GetCoinsBest();
        void SetCoinsBest(int coinsBest);
    }
}