using Inputs;
using UnityEngine;

public class MazeDiBootstrap : MonoBehaviour
{
    private MonoBehaviourMethodsCaller monobehMethods = new MonoBehaviourMethodsCaller();
    
    private void Awake()
    {
        BootstrapDependencyInjection();
        RegisterMonobehListeners();
        monobehMethods.Awake();
    }

    private void Update()
    {
        this.monobehMethods.Update();
    }

    private void OnDestroy()
    {
        this.monobehMethods.OnDestroy();
    }

    private void OnApplicationQuit()
    {
        UnloadAndCleanAll();
    }

    public void UnloadAndCleanAll()
    {
        DI.DisposeOfGameContainer();
    }

    private void BootstrapDependencyInjection()
    {
        DI.CreateGameContainer();
        DI.Game.Register<IInput, InputBridge>().AsSingleton();
        DI.Game.Register<LevelTimer>().AsSingleton();
        DI.Game.Register<LevelStateDispatcher>().AsSingleton();
        DI.Game.Register<ILevelStateInfoProvider>((c, p) => c.Resolve<LevelStateDispatcher>());
    }

    private void RegisterMonobehListeners()
    {
        this.monobehMethods.Register(DI.Game.Resolve<LevelTimer>());
    }
}