using LevelMechanics;
using MazeMechanics;
using MazeMechanics.Cells;
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
        [SerializeField]
        private PlayerView playerView;
        
        public override void RegisterDependencies(TinyIoCContainer container)
        {
            this.mazeCellFactory.Inject(this.collectableFactory);
            container.Register<ICollectableViewFactory, CollectableViewFactory>(this.collectableFactory);
            container.Register<IMazeCellViewFactory, MazeCellViewFactory>(this.mazeCellFactory);
            
            container.Register<IPlayerView>(this.playerView);
        }
    }
}