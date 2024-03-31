using LevelMechanics;
using MazeMechanics;
using TinyIoC;
using UnityEngine;

namespace Views
{
    public class LevelViewsDiContainerPopulator : DiContainerPopulator
    {
        [SerializeField]
        private CollectableViewFactory collectableFactory;
        [SerializeField]
        private MazeCellViewFactory mazeCellFactory;
        
        public override void RegisterDependencies(TinyIoCContainer container)
        {
            this.mazeCellFactory.Inject(this.collectableFactory);
            DI.Game.Register<ICollectableViewFactory, CollectableViewFactory>(this.collectableFactory);
            DI.Game.Register<IMazeCellViewFactory, MazeCellViewFactory>(this.mazeCellFactory);
        }
    }
}