using Inputs;
using LevelMechanics;
using TinyIoC;

namespace MazeMechanics
{
    public class LevelMechanicsDiContainerPopulator : DiContainerPopulator
    {
        public override void RegisterDependencies(TinyIoCContainer container)
        {
            container.Register<IInput, InputBridge>().AsSingleton();
            container.Register<LevelTimer>().AsSingleton();
            container.Register<LevelStateDispatcher>().AsSingleton();
            container.Register<ILevelStateInfoProvider>((c, p) => c.Resolve<LevelStateDispatcher>());

            container.Register<DraftCellController>();

            container.Register<MazeCellPresenter>();
            container.Register<ICollectablePresenterFactory, CollectableManager>();

            container.Register<CollectableRefresher>().AsSingleton();
        }

        public override void RegisterMonobehListeners(MonoBehaviourMethodsCaller monoBehCaller)
        {
            monoBehCaller.Register(DI.Game.Resolve<LevelTimer>());
            monoBehCaller.Register(DI.Game.Resolve<CollectableRefresher>());
        }
    }
}