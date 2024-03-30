using MazeMechanics;
using UnityEngine;

namespace Views
{
    public class MazeViewsDiBootstrap : MonoBehaviour
    {
        [SerializeField]
        private CollectibleViewFactory collectibleFactory;
        [SerializeField]
        private MazeCellViewFactory mazeCellFactory;

        private MonoBehaviourMethodsCaller monoBehMethods = new();
        private MazeMechanicsDiBootstrap mechanicsDi;

        private void Awake()
        {
            DI.CreateGameContainer();
        
            this.monoBehMethods = new MonoBehaviourMethodsCaller();
            this.mechanicsDi = new MazeMechanicsDiBootstrap(this.monoBehMethods);
        
            DI.Game.Register<ICollectibleViewFactory, CollectibleViewFactory>(this.collectibleFactory);
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