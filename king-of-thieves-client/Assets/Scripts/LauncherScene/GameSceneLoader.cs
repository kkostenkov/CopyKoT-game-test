using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    private async void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        var delay = 1f;
        await Task.Delay(TimeSpan.FromSeconds(delay));

        SceneManager.LoadScene(1);
    }
}