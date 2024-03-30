using System.Threading.Tasks;
using UnityEngine;

public class MazeFlow : MonoBehaviour
{
    private LevelTimer timer;
    private LevelStateDispatcher levelState;

    private async Task Start()
    {
        CacheDependencies();
        InitiateLevelTimer();
        StartLevel();
    }

    private void CacheDependencies()
    {
        this.timer = DI.Game.Resolve<LevelTimer>();
        this.levelState = DI.Game.Resolve<LevelStateDispatcher>();
    }

    private void StartLevel()
    {
        this.levelState.Set(LevelState.Action);
    }

    private void StopLevel()
    {
        this.levelState.Set(LevelState.TimeIsUp);
    }

    private void InitiateLevelTimer()
    {
        this.timer.Expired += OnLevelTimeEnded;
        this.timer.Start();
    }

    private void OnLevelTimeEnded()
    {
        this.timer.Expired -= OnLevelTimeEnded;
        Debug.Log("Level timer expired");
        StopLevel();
    }
}