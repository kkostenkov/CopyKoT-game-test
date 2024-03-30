using DYP;
using UnityEngine;

namespace Controllers.Modules
{
    [System.Serializable]
    internal class CustomActionModule
    {
        public BaseActionModule Module;

        private int actionDir;

        public int ActionDir {
            get { return this.actionDir; }
        }

        private float m_ActionTimer;
        private float m_PrevActionTimer;

        public void Start(int actionDir)
        {
            this.actionDir = actionDir;
            this.m_PrevActionTimer = 0.0f;
            this.m_ActionTimer = 0.0f;
        }

        public void CustomUpdate(float timeStep)
        {
            this.m_PrevActionTimer = this.m_ActionTimer;
            this.m_ActionTimer += timeStep;

            if (this.m_ActionTimer > this.Module.ActionTime) {
                this.m_ActionTimer = this.Module.ActionTime;
            }
        }

        public Vector2 GetActionVelocity()
        {
            if (this.Module != null) {
                return this.Module.GetActionSpeed(this.m_PrevActionTimer, this.m_ActionTimer);
            }
            else {
                return Vector2.zero;
            }
        }

        public float GetActionProgress()
        {
            return this.Module.GetActionProgress(this.m_ActionTimer);
        }
    }
}