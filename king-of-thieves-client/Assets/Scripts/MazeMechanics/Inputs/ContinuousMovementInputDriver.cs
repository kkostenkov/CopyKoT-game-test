using Controllers;
using DYP;
using UnityEngine;

namespace Inputs
{
    public class ContinuousMovementInputDriver : BaseInputDriver
    {
        [SerializeField]
        private BaseMovementController movementController;

        private Vector2 horizontalMovement = Vector2.right;
        private bool couldChangeDirection;
        private IInput input;

        private void Awake()
        {
            this.movementController.WallSliding += OnWallSliding;
            this.movementController.WallSlided += OnWallSlideEnded;
        }

        private void Start()
        {
            this.input = DI.Game.Resolve<IInput>();
        }

        private void Update()
        {
            UpdateInput(Time.deltaTime);
        }

        private void OnDestroy()
        {
            this.movementController.WallSliding -= OnWallSliding;
            this.movementController.WallSlided -= OnWallSlideEnded;
        }

        private void OnWallSlideEnded()
        {
            this.couldChangeDirection = false;
        }

        private void OnWallSliding(int direction)
        {
            this.couldChangeDirection = true;
        }

        public override void UpdateInput(float timeStep)
        {
            var isJumpPressed = this.input.IsJumpRequested;
            Jump = isJumpPressed;
        
            if (isJumpPressed && this.couldChangeDirection) {
                InvertHorizontalMovementDirection();
            }
        
            Horizontal = this.horizontalMovement.x;
        }

        private void InvertHorizontalMovementDirection()
        {
            this.horizontalMovement *= -1;
        }
    }
}