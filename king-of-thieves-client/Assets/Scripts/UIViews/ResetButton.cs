using MazeMechanics;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    private LevelStateDispatcher levelState;

    private void Start()
    {
        this.levelState = DI.Game.Resolve<LevelStateDispatcher>();
    }

    public void ResetUiButtonCallback()
    {
        levelState.Set(LevelState.SessionEnded);
    }
}
