using System.Threading.Tasks;
using LevelMechanics.UI;
using UnityEngine;

public class LevelUiManager : MonoBehaviour, ILevelUiManager
{
    private void Start()
    {
        var levelInfoPresenter = DI.Game.Resolve<LevelInfoPresenter>();
        var coinsPresenter = DI.Game.Resolve<CoinsPresenter>();
        var timePresenter = DI.Game.Resolve<LevelTimePresenter>();
    }

    public Task Build()
    {
        return Task.CompletedTask;
    }
}