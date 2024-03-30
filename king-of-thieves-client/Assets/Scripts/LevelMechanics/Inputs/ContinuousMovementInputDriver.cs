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
        private IInput input;

        private void Awake()
        {
            this.movementController.WallJumpExecuted += OnWallJumpExecuted;
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
            this.movementController.WallJumpExecuted -= OnWallJumpExecuted;
        }

        private void OnWallJumpExecuted(Vector2 obj)
        {
            InvertHorizontalMovementDirection();
        }

        public override void UpdateInput(float timeStep)
        {
            var isJumpPressed = this.input.IsJumpRequested;
            Jump = isJumpPressed;
            
            Horizontal = this.horizontalMovement.x;
        }

        private void InvertHorizontalMovementDirection()
        {
            this.horizontalMovement *= -1;
        }
    }
}