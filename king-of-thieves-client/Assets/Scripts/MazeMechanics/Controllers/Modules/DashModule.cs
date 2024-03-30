using DYP;

namespace Controllers.Modules
{
    [System.Serializable]
    public class DashModule
    {
        public BaseDashModule Module;

        private int dashDir;

        public int DashDir {
            get { return this.dashDir; }
        } // 1: right, -1: left

        private float dashTimer;
        private float prevDashTimer;

        public void Start(int dashDir, float timeStep)
        {
            this.dashDir = dashDir;
            this.prevDashTimer = 0.0f;
            this.dashTimer = timeStep;
        }

        public void CustomUpdate(float timeStep)
        {
            this.prevDashTimer = this.dashTimer;
            this.dashTimer += timeStep;

            if (this.dashTimer > this.Module.DashTime) {
                this.dashTimer = this.Module.DashTime;
            }
        }

        public float GetDashSpeed()
        {
            if (this.Module != null) {
                return this.Module.GetDashSpeed(this.prevDashTimer, this.dashTimer);
            }
            else {
                return 0;
            }
        }

        public float GetDashProgress()
        {
            return this.Module.GetDashProgress(this.dashTimer);
        }
    }
}