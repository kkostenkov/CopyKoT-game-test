using System.Collections.Generic;
using UnityEngine;

namespace LevelMechanics
{
    public class DiBootstrapper : MonoBehaviour
    {
        [SerializeReference]
        public List<DiContainerPopulator> DiPopulators;
        
        private MonoBehaviourMethodsCaller monoBehMethods = new();

        private void Awake()
        {
            DI.CreateGameContainer();
            foreach (var diContainerPopulator in this.DiPopulators) {
                diContainerPopulator.RegisterDependencies(DI.Game);
                diContainerPopulator.RegisterMonobehListeners(this.monoBehMethods);
            }
            this.monoBehMethods.Awake();
        }

        public void Update()
        {
            this.monoBehMethods.Update();
        }

        public void OnDestroy()
        {
            this.monoBehMethods.OnDestroy();
            UnloadAndCleanAll();
        }
        
        // private void OnApplicationQuit()
        // {
        //     UnloadAndCleanAll();
        // }
        
        public void UnloadAndCleanAll()
        {
            DI.DisposeOfGameContainer();
        }
    }
}
