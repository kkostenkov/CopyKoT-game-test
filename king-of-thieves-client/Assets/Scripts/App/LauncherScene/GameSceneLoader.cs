using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        StartCoroutine(DalayLoading());
    }

    private IEnumerator DalayLoading()
    {
        var delay = 1f;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }
}