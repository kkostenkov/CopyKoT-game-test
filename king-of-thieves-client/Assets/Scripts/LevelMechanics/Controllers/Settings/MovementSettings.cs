namespace Controllers.Settings
{
    [System.Serializable]
    class MovementSettings
    {
        public float Speed = 8;
        public float AccelerationTimeAirborne = .1f;
        public float AccelerationTimeGrounded = .1f;
    }
}