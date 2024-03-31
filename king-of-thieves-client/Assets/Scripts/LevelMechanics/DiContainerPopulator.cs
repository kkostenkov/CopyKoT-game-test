using TinyIoC;
using UnityEngine;

namespace LevelMechanics
{
    public class DiContainerPopulator : MonoBehaviour
    {
        public virtual void RegisterDependencies(TinyIoCContainer container)
        {
        }

        public virtual void RegisterMonobehListeners(MonoBehaviourMethodsCaller monoBehCaller)
        {
        }
    }
}