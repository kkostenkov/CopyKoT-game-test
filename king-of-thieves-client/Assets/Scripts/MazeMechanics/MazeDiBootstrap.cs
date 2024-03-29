using UnityEngine;

public class DiBootstrapViews : MonoBehaviour
{
    private void Awake()
    {
        BootstrapDependencyInjection();
    }

    private void BootstrapDependencyInjection()
    {
        DI.CreateGameContainer();
        DI.Game.Register<IInput, InputBridge>().AsSingleton();
    }

    private void OnApplicationQuit()
    {
        UnloadAndCleanAll();
    }

    public void UnloadAndCleanAll()
    {
        DI.DisposeOfGameContainer();
    }
}