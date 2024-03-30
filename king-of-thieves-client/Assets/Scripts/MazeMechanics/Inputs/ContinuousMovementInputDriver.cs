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
            this.movementController.WallSliding += WallSlide;
            this.movementController.WallSlided += WallSlideEnd;
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
            this.movementController.WallSliding -= WallSlide;
            this.movementController.WallSlided -= WallSlideEnd;
        }

        private void WallSlideEnd()
        {
            this.couldChangeDirection = false;
        }

        private void WallSlide(int obj)
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