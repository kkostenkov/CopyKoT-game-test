using LevelMechanics;
using LevelMechanics.UI;
using TinyIoC;
using UnityEngine;

public class UIViewsDiContainerPopulator : DiContainerPopulator
{
    [SerializeField]
    private LevelInfoView levelInfoView;
    [SerializeField]
    private CoinsView coinsView;
    [SerializeField]
    private LevelTimeView levelTimeView;

    [SerializeField]
    private LevelUiManager uiManager;

    public override void RegisterDependencies(TinyIoCContainer container)
    {
        container.Register<ILevelInfoView>(levelInfoView);
        container.Register<ICoinsView>(coinsView);
        container.Register<ILevelTimeView>(levelTimeView);
        container.Register<ILevelUiManager>(uiManager);
    }
}
