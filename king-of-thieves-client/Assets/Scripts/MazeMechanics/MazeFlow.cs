using System.Threading.Tasks;
using UnityEngine;

public class MazeFlow : MonoBehaviour
{
    private LevelTimer timer;

    private async Task Start()
    {
        InitiateLevelTimer();
    }

    private void InitiateLevelTimer()
    {
        this.timer = DI.Game.Resolve<LevelTimer>();
        this.timer.Expired += OnLevelTimeEnded;
        this.timer.Start();
    }

    private void OnLevelTimeEnded()
    {
        this.timer.Expired -= OnLevelTimeEnded;
        Debug.Log("Level timer expired");
    }
}