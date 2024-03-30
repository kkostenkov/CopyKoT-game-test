using Inputs;

namespace MazeMechanics
{
    public class MazeMechanicsDiBootstrap
    {
        private MonoBehaviourMethodsCaller monoBehMethods;

        public MazeMechanicsDiBootstrap(MonoBehaviourMethodsCaller monoBehMethods)
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
            DI.Game.Register<CollectablePresenter>();
        }

        private void RegisterMonobehListeners()
        {
            this.monoBehMethods.Register(DI.Game.Resolve<LevelTimer>());
        }
    }
}