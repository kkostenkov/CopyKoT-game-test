namespace Controllers.Settings
{
    [System.Serializable]
    class JumpSettings
    {
        public bool HasVariableJumpHeight = true;
        public int AirJumpAllowed = 1;
        public float MaxHeight = 3.5f;
        public float MinHeight = 1.0f;
        public float TimeToApex = .4f;
        public float OnLadderJumpForce = 0.4f;

        public float FallingJumpPaddingTime = 0.09f;
        public float WillJumpPaddingTime = 0.15f;
    }
}