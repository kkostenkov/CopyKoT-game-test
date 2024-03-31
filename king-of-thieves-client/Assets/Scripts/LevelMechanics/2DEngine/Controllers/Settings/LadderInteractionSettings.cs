namespace Controllers.Settings
{
    [System.Serializable]
    class LadderInteractionSettings
    {
        public bool SnapToRestrictedArea = false;
        public bool ExitLadderOnGround = true;
        public float OnLadderSpeed = 4;
        public bool LockFacingToRight = true;
    }
}