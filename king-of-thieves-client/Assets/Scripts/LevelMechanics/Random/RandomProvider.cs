namespace MazeMechanics.Random
{
    class RandomProvider : IRandomProvider
    {
        private System.Random rand = new System.Random();
        
        public int RollPercent()
        {
            return this.rand.Next(0, 100);
        }

        public bool CheckPercentChance(int percent)
        {
            return percent < RollPercent();
        }

        public double Next(int min, int max)
        {
            return this.rand.Next(min, max);
        }
    }
}