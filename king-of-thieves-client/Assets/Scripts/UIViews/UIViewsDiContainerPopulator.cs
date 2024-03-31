using LevelMechanics;
using LevelMechanics.UI;
using TinyIoC;
using UnityEngine;

public class UIViewsDiContainerPopulator : DiContainerPopulator
{
    [SerializeField]
    private LevelInfoView levelInfoView;
    
    public override void RegisterDependencies(TinyIoCContainer container)
    {
        container.Register<ILevelInfoView>(levelInfoView);
    }
}
