using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class LevelViewsDiBootstrap : MonoBehaviour
    {
        [SerializeField]
        private CollectableViewFactory collectableFactory;
        [SerializeField]
        private MazeCellViewFactory mazeCellFactory;

        private MonoBehaviourMethodsCaller monoBehMethods = new();
        private LevelMechanicsDiBootstrap mechanicsDi;

        private void Awake()
        {
            DI.CreateGameContainer();
        
            this.monoBehMethods = new MonoBehaviourMethodsCaller();
            this.mechanicsDi = new LevelMechanicsDiBootstrap(this.monoBehMethods);

            this.mazeCellFactory.Inject(this.collectableFactory);
            DI.Game.Register<ICollectableViewFactory, CollectableViewFactory>(this.collectableFactory);
            DI.Game.Register<IMazeCellViewFactory, MazeCellViewFactory>(this.mazeCellFactory);

            this.monoBehMethods.Awake();
        }
    
        public void Update()
        {
            this.monoBehMethods.Update();
        }

        public void OnDestroy()
        {
            this.monoBehMethods.OnDestroy();
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
}