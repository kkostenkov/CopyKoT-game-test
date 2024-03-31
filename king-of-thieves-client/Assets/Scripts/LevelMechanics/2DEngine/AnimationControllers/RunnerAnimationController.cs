using System;
using Controllers;
using DYP;
using UnityEngine;

namespace LevelMechanics
{
    public class RunnerAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Transform appearance;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private BaseMovementController movementController;
        [SerializeField]
        private BaseMotor2D motor;

        private static readonly int SpeedYAnimatorParam = Animator.StringToHash("SpeedY");
        private static readonly int IsGroundedAnimatorParam = Animator.StringToHash("IsGrounded");
        private static readonly int IsOnWallAnimatorParam = Animator.StringToHash("IsOnWall");
        private static readonly int SpeedXAnimatorParam = Animator.StringToHash("SpeedX");

        private void Start()
        {
            this.movementController.FacingFlipped += OnFacingFlipped;
        }

        private void Update()
        {
            var motorVelocity = this.motor.Velocity;
            if (AlmostEquals(motorVelocity.x, 0, 0.01f)) {
                this.animator.SetFloat(SpeedXAnimatorParam, 0.0f);
            }
            else {
                this.animator.SetFloat(SpeedXAnimatorParam, 1.0f);
            }
        
            this.animator.SetFloat(SpeedYAnimatorParam, this.movementController.InputVelocity.y);

            this.animator.SetBool(IsGroundedAnimatorParam, this.movementController.IsOnGround());

            this.animator.SetBool(IsOnWallAnimatorParam, this.movementController.IsState(MotorState.WallSliding));
        }
    
        private void OnDestroy()
        {
            this.movementController.FacingFlipped -= OnFacingFlipped;
        }

        public static bool AlmostEquals(float double1, float double2, float precision)
        {
            return (Math.Abs(double1 - double2) <= precision);
        }

        private void OnFacingFlipped(int facing)
        {
            this.appearance.localScale = new Vector3(facing, 1, 1);
        }
    }
}