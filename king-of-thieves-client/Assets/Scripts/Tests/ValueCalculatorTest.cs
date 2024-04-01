using MazeMechanics;
using NUnit.Framework;

public class ValueCalculatorTest
{
   [Test]
   public void Should_GiveNothing_WhenNoTreasure()
   {
      var coinBalance = 0;
      var model = new CollectableModel() {
         Treasure = TreasureKind.None
      };

      var value = ValueCalculator.GetCoinValue(model, coinBalance);

      Assert.AreEqual(0, value);
   }
   
   [Test]
   public void Should_GiveOne_WhenGivenCoin()
   {
      var coinBalance = 0;
      var model = new CollectableModel() {
         Treasure = TreasureKind.Coin
      };

      var value = ValueCalculator.GetCoinValue(model, coinBalance);

      Assert.AreEqual(1, value);
   }
   
   [TestCase(0, ExpectedResult = 1)]
   [TestCase(1, ExpectedResult = 1)]
   [TestCase(10, ExpectedResult = 1)]
   [TestCase(11, ExpectedResult = 1)]
   [TestCase(19, ExpectedResult = 1)]
   [TestCase(20, ExpectedResult = 2)]
   [TestCase(55, ExpectedResult = 5)]
   [TestCase(99, ExpectedResult = 9)]
   [TestCase(14590, ExpectedResult = 1459)]
   public int Should_GiveTenPercentOfBalance_WhenGivenChest(int coinBalance)
   {
      var model = new CollectableModel() {
         Treasure = TreasureKind.Chest
      };

      var value = ValueCalculator.GetCoinValue(model, coinBalance);

      return value;
   }
}
