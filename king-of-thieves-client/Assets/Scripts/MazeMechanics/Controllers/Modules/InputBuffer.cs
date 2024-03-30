using UnityEngine;

namespace Controllers.Modules
{
    [System.Serializable]
    class InputBuffer
    {
        public Vector2 Input = Vector2.zero;

        public bool IsJumpPressed = false;
        public bool IsJumpHeld = false;
        public bool IsJumpReleased = false;
        public bool IsDashPressed = false;
        public bool IsDashHeld = false;
    }
}