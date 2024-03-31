using UnityEngine;

namespace Controllers.Settings
{
    [System.Serializable]
    class WallInteractionSettings
    {
        public bool CanWallSlide = true;
        public float WallStickTime = 0.15f;

        [HideInInspector]
        public float WallStickTimer = 0.0f;

        public float WallSlideSpeedLoss = 0.05f;
        public float WallSlidingSpeedMax = 2;

        public bool CanWallClimb = false;
        public float WallClimbSpeed = 2;

        public bool CanWallJump = true;
        public Vector2 ClimbForce = new Vector2(12, 16);
        public Vector2 OffForce = new Vector2(8, 15);
        public Vector2 LeapForce = new Vector2(18, 17);

        public bool CanGrabLedge = false;
        public float LedgeDetectionOffset = 0.1f;

        [HideInInspector]
        public int WallDirX = 0;
    }
}