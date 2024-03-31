using UnityEngine;

namespace Inputs
{
    public class InputBridge : IInput
    {
        public bool IsJumpRequested => GetButtonJumpDown() || GetTapJumpDown();

        private static bool GetTapJumpDown()
        {
            if (Input.touchCount <= 0) {
                return false;
            }

            for (int i = 0; i < Input.touchCount; i++) {
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    return true;
                }
            }

            return false;
        }

        private static bool GetButtonJumpDown()
        {
            return Input.GetButtonDown("Jump");
        }
    }
}