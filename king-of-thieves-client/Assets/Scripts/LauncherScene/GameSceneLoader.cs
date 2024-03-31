using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    private async void Awake()
    {
        var delay = Application.isEditor ? 0 : 0.5f;
        await Task.Delay(TimeSpan.FromSeconds(delay));

        SceneManager.LoadScene(1);
    }
}