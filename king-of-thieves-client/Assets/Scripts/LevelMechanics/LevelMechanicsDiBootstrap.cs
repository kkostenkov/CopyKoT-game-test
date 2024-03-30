using Inputs;

namespace MazeMechanics
{
    public class LevelMechanicsDiBootstrap
    {
        private MonoBehaviourMethodsCaller monoBehMethods;

        public LevelMechanicsDiBootstrap(MonoBehaviourMethodsCaller monoBehMethods)
        {
            this.monoBehMethods = monoBehMethods;
            BootstrapDependencyInjection();
            RegisterMonobehListeners();
        }

        private void BootstrapDependencyInjection()
        {
            DI.Game.Register<IInput, InputBridge>().AsSingleton();
            DI.Game.Register<LevelTimer>().AsSingleton();
            DI.Game.Register<LevelStateDispatcher>().AsSingleton();
            DI.Game.Register<ILevelStateInfoProvider>((c, p) => c.Resolve<LevelStateDispatcher>());
        
            DI.Game.Register<DraftCellController>();
        
            DI.Game.Register<MazeCellPresenter>();
            DI.Game.Register<ICollectablePresenterFactory, CollectableManager>();
            
            DI.Game.Register<CollectableRefresher>().AsSingleton();
        }

        private void RegisterMonobehListeners()
        {
            this.monoBehMethods.Register(DI.Game.Resolve<LevelTimer>());
            this.monoBehMethods.Register(DI.Game.Resolve<CollectableRefresher>());
        }
    }
}