using LevelMechanics.Settings;
using MazeMechanics;
using MazeMechanics.Cells;
using MazeMechanics.Random;
using NSubstitute;
using NUnit.Framework;

public class TreasureGeneratorTest
{
    private ITreasureGenerationSettingsProvider settings;
    private IRandomProvider random;
    private TreasureGenerator treasureGen;
    
    [SetUp]
    public void SetupTest()
    {
        settings = Substitute.For<ITreasureGenerationSettingsProvider>();
        random = Substitute.For<IRandomProvider>();
        treasureGen = new TreasureGenerator(settings, random);
    }
    
    [Test]
    public void Should_AddTreasure_When_GivenTileThatCouLdContainCollectables()
    {
        var model = new MazeCellModel() {
            CouldContainCollectables = true,
        };

        treasureGen.TryAddTreasure(model);
        
        Assert.IsNotNull(model.CollectableModel);
        Assert.IsTrue(model.CollectableModel.Treasure != TreasureKind.None);
    }
    
    [Test]
    public void Should_LeaveEmpty_When_GivenTileThatCouldNotContainCollectables()
    {
        var model = new MazeCellModel() {
            CouldContainCollectables = false,
        };

        treasureGen.TryAddTreasure(model);
        
        Assert.IsNotNull(model.CollectableModel);
        Assert.IsTrue(model.CollectableModel.Treasure == TreasureKind.None);
    }

    [Test]
    public void Should_CreateChest_WhenChestChanceIs100Percent()
    {
        settings.ChestChancePercent.Returns(100);
        
        var model = treasureGen.GenerateTreasure();
        
        Assert.IsTrue(model.Treasure == TreasureKind.Chest);
    }
    
    [Test]
    public void Should_CreateCoin_WhenChestChanceIs0Percent()
    {
        settings.ChestChancePercent.Returns(0);
        
        var model = treasureGen.GenerateTreasure();
        
        Assert.IsTrue(model.Treasure == TreasureKind.Coin);
    }
    
    [Test]
    public void Should_LeaveNoTreasure_When_AskedToClearModel()
    {
        var model = new CollectableModel() {
            Treasure = TreasureKind.Chest,
        };
        
        treasureGen.Clear(model);
        
        Assert.IsTrue(model.Treasure == TreasureKind.None);
    }
}
