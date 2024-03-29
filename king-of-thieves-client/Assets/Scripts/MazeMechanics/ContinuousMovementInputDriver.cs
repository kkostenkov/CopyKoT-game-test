using DYP;
using UnityEngine;

public class ContinuousMovementInputDriver : BaseInputDriver
{
    [SerializeField]
    private BasicMovementController2D movementController;

    private Vector2 horizontalMovement = Vector2.right;
    private bool isWallSliding;

    private void Awake()
    {
        movementController.OnWallSliding += OnWallSlide;
        movementController.OnWallSlidingEnd += OnWallSlideEnd;
    }

    private void Update()
    {
        UpdateInput(Time.deltaTime);
    }

    private void OnDestroy()
    {
        movementController.OnWallSliding -= OnWallSlide;
        movementController.OnWallSlidingEnd -= OnWallSlideEnd;
    }

    private void OnWallSlideEnd()
    {
        this.isWallSliding = false;
    }

    private void OnWallSlide(int obj)
    {
        this.isWallSliding = true;
    }

    public override void UpdateInput(float timeStep)
    {
        var isJumpPressed = Input.GetButtonDown("Jump");
        Jump = isJumpPressed;
        
        if (isJumpPressed && this.isWallSliding) {
            InvertHorizontalMovementDirection();
        }

        Horizontal = this.horizontalMovement.x;

        HoldingJump = Input.GetButton("Jump");
        ReleaseJump = Input.GetButtonUp("Jump");
    }

    private void InvertHorizontalMovementDirection()
    {
        this.horizontalMovement *= -1;
    }
}