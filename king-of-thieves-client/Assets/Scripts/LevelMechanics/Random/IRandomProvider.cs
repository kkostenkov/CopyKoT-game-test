namespace MazeMechanics.Random
{
    public interface IRandomProvider
    {
        int RollPercent();
        bool CheckPercentChance(int percent);
        double Next(int minDelaySeconds, int maxDelaySeconds);
    }
}