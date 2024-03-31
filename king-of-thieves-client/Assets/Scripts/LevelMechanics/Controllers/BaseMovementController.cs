using System;
using Controllers.Modules;
using Controllers.Settings;
using DYP;
using MazeMechanics;
using UnityEngine;

namespace Controllers
{
    /*
     *  Handles Gravity, Jump and Horizontal Movement
     */
    
    [RequireComponent(typeof(CharacterMotor2D))]
    public class BaseMovementController : MonoBehaviour
    {
        [SerializeField]
        private CharacterMotor2D motor;
        [SerializeField]
        private BaseInputDriver inputDriver;
        [Header("Settings")]
        [SerializeField]
        private MovementSettings movementSettings;
        [SerializeField]
        private LadderInteractionSettings ladderInteractionSettings;
        [SerializeField]
        private JumpSettings jumpSettings;
        [SerializeField]
        private WallInteractionSettings wallInteractionSettings;
        [SerializeField]
        private DashModule dashModule = new DashModule();
        [SerializeField]
        private CustomActionModule customActionModule = new CustomActionModule();
    
        private Collider2D MotorCollider => this.motor.Collider2D;
        private MotorState motorState;
    
        private readonly InputBuffer inputBuffer = new InputBuffer();

        private float gravity;
        private bool applyGravity = true;

        private float maxJumpSpeed;
        private float minJumpSpeed;

        private int airJumpCounter = 0;
        private int willJumpPaddingFrame = -1;
        private int fallingJumpPaddingFrame = -1;

        private float currentTimeStep = 0;

        private int facingDirection = 1;

        public int FacingDirection {
            get { return this.facingDirection; }
            private set {
                int oldFacing = this.facingDirection;
                this.facingDirection = value;

                if (this.facingDirection != oldFacing) {
                    this.FacingFlipped(this.facingDirection);
                }
            }
        }

        public BaseDashModule DashModule {
            get => this.dashModule.Module;
            private set => this.dashModule.Module = value;
        }

        public BaseActionModule ActionModule {
            get => this.customActionModule.Module;
            private set => this.customActionModule.Module = value;
        }

        private readonly OnLadderState onLadderState = new OnLadderState();

        [Header("State")]
        private Vector3 velocity;

        public Vector3 InputVelocity {
            get { return this.velocity; }
        }

        public float MovementSpeed {
            get => this.movementSettings.Speed;
            set => this.movementSettings.Speed = value;
        }

        private float velocityXSmoothing;

        // Action
        public event Action<MotorState, MotorState> OnMotorStateChanged = delegate { };

        public event System.Action OnJump = delegate { }; // on all jump! // OnEnterStateJump
        public event System.Action OnJumpEnd = delegate { }; // on jump -> falling  // OnLeaveStateJump

        public event System.Action OnNormalJump = delegate { }; // on ground jump
        public event System.Action OnLedgeJump = delegate { }; // on ledge jump
        public event System.Action OnLadderJump = delegate { }; // on ladder jump
        public event System.Action OnAirJump = delegate { }; // on air jump
        public event System.Action<Vector2> WallJumpExecuted = delegate { }; // on wall jump

        public event System.Action<int> OnDash = delegate { }; // int represent dash direction
        public event System.Action<float> OnDashStay = delegate { }; // float represent action progress
        public event System.Action OnDashEnd = delegate { };

        public event System.Action<int> WallSliding = delegate { }; // int represnet wall direction: 1 -> right, -1 -> left
        public event System.Action WallSlided = delegate { };

        public event System.Action<int> OnLedgeGrabbing = delegate { };
        public event System.Action OnLedgeGrabbingEnd = delegate { };

        public event System.Action OnLanded = delegate { }; // on grounded

        public event System.Action<MotorState> OnResetJumpCounter = delegate { };
        public event System.Action<int> FacingFlipped = delegate { };

        public event System.Action<int> OnAction = delegate { };
        public event System.Action<float> OnActionStay = delegate { };
        public event System.Action OnActionEnd = delegate { };

        // Condition
        public Func<bool> CanAirJumpFunc = null;
        private ILevelStateInfoProvider levelInfo;

#region Monobehaviour
    
        private void Start()
        {
            this.levelInfo = DI.Game.Resolve<ILevelStateInfoProvider>();
            Init();
        }

        private void FixedUpdate()
        {
            if (this.levelInfo.CurrentState != LevelState.SessionStarted) {
                return;
            }
            
            RunControllerCycle(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (this.levelInfo.CurrentState != LevelState.SessionStarted) {
                return;
            }
            ReadInput(Time.deltaTime);
        }

#endregion

        public void SetFrozen(bool freeze)
        {
            if (freeze) {
                this.velocity.x = 0;
                ChangeState(MotorState.Frozen);
            }
            else {
                if (IsOnGround()) {
                    ChangeState(MotorState.OnGround);
                }
                else {
                    ChangeState(MotorState.Falling);
                }
            }
        }

        public bool IsState(MotorState state)
        {
            return this.motorState == state;
        }

        public bool IsInLadderArea()
        {
            return this.onLadderState.IsInLadderArea;
        }

        public bool IsInLadderTopArea()
        {
            return this.onLadderState.IsInLadderArea && this.onLadderState.AreaZone == LadderZone.Top;
        }

        public bool IsOnGround()
        {
            return this.motor.Collisions.Below;
        }

        public bool IsInAir()
        {
            return !this.motor.Collisions.Below && !this.motor.Collisions.Left &&
                   !this.motor.Collisions.Right; //IsState(MotorState.Jumping) || IsState(MotorState.Falling);
        }

        public bool IsAgainstWall()
        {
            if (this.motor.Collisions.Left) {
                float leftWallAngle = Vector2.Angle(this.motor.Collisions.LeftNormal, Vector2.right);
                if (leftWallAngle < 0.01f) {
                    return true;
                }
            }

            if (this.motor.Collisions.Right) {
                float rightWallAngle = Vector2.Angle(this.motor.Collisions.RightNormal, Vector2.left);
                if (rightWallAngle < 0.01f) {
                    return true;
                }
            }

            return false;
        }

        public bool CheckIfAtLedge(int wallDirX, ref Vector2 ledgePoint)
        {
            // first raycast down, then check overlap
            var boundingBox = MotorCollider.bounds;

            Vector2 origin = Vector2.zero;
            origin.y = boundingBox.max.y + this.wallInteractionSettings.LedgeDetectionOffset;

            // right wall
            if (wallDirX == 1) {
                origin.x = boundingBox.max.x + this.wallInteractionSettings.LedgeDetectionOffset;
            }
            // left wall
            else if (wallDirX == -1) {
                origin.x = boundingBox.min.x - this.wallInteractionSettings.LedgeDetectionOffset;
            }

            float distance = this.wallInteractionSettings.LedgeDetectionOffset * 2;
            float speedY = -this.velocity.y * this.currentTimeStep;
            distance = (speedY > distance) ? speedY : distance;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, this.motor.Raycaster.CollisionLayer);

            ledgePoint = hit.point;

            if (hit.collider != null) {
                Bounds overlapBox = new Bounds(
                    new Vector3(ledgePoint.x, ledgePoint.y) + Vector3.up * this.wallInteractionSettings.LedgeDetectionOffset,
                    Vector3.one * this.wallInteractionSettings.LedgeDetectionOffset);

                Collider2D col = Physics2D.OverlapArea(overlapBox.min, overlapBox.max);
                return (col == null);
            }
            else {
                return false;
            }
        }

        public bool CanAirJump()
        {
            if (this.CanAirJumpFunc != null) {
                return this.CanAirJumpFunc();
            }
            else {
                return this.airJumpCounter < this.jumpSettings.AirJumpAllowed;
            }
        }

        public void ChangeDashModule(BaseDashModule module, bool disableDashingState = false)
        {
            if (module != null) {
                DashModule = module;

                if (disableDashingState) {
                    ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
                }
            }
            else {
                DashModule = null;
                ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
            }
        }

        public void ChangeActionModule(BaseActionModule module, bool disableActionState = false)
        {
            if (module != null) {
                ActionModule = module;

                if (disableActionState) {
                    ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
                }
            }
            else {
                ActionModule = null;
                ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
            }
        }

        public void LadderAreaEnter(Bounds area, float topAreaHeight, float bottomAreaHeight)
        {
            this.onLadderState.IsInLadderArea = true;
            this.onLadderState.Area = area;

            this.onLadderState.TopArea = new Bounds(
                new Vector3(area.center.x, area.center.y + area.extents.y - topAreaHeight / 2, 0),
                new Vector3(area.size.x, topAreaHeight, 100)
            );

            this.onLadderState.BottomArea = new Bounds(
                new Vector3(area.center.x, area.center.y - area.extents.y + bottomAreaHeight / 2, 0),
                new Vector3(area.size.x, bottomAreaHeight, 100)
            );
        }

        public void LadderAreaExit()
        {
            this.onLadderState.IsInLadderArea = false;

            this.onLadderState.Area = new Bounds(Vector3.zero, Vector3.zero);
            this.onLadderState.TopArea = new Bounds(Vector3.zero, Vector3.zero);
            this.onLadderState.BottomArea = new Bounds(Vector3.zero, Vector3.zero);

            if (IsState(MotorState.OnLadder)) {
                ExitLadderState();
            }
        }

        public void SetLadderRestrictedArea(Bounds b, bool isTopIgnored = false)
        {
            this.onLadderState.HasRestrictedArea = true;

            this.onLadderState.RestrictedArea = b;
            this.onLadderState.RestrictedAreaTopRight = b.center + b.extents;
            this.onLadderState.RestrictedAreaBottomLeft = b.center - b.extents;

            if (isTopIgnored) {
                this.onLadderState.RestrictedAreaTopRight.y = Mathf.Infinity;
            }
        }

        public void SetLadderZone(LadderZone zone)
        {
            this.onLadderState.AreaZone = zone;
        }

        public void ClearLadderRestrictedArea()
        {
            this.onLadderState.HasRestrictedArea = false;
        }

        public bool IsRestrictedOnLadder()
        {
            return this.onLadderState.HasRestrictedArea;
        }

        public void Init()
        {
            // S = V0 * t + a * t^2 * 0.5
            // h = V0 * t + g * t^2 * 0.5
            // h = g * t^2 * 0.5
            // g = h / (t^2*0.5)

            this.gravity = -this.jumpSettings.MaxHeight / (this.jumpSettings.TimeToApex * this.jumpSettings.TimeToApex * 0.5f);
            this.maxJumpSpeed = Mathf.Abs(this.gravity) * this.jumpSettings.TimeToApex;
            this.minJumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(this.gravity) * this.jumpSettings.MinHeight);
            this.airJumpCounter = 0;

            this.motor.OnMotorCollisionEnter2D += OnMotorCollisionEnter2D;
            this.motor.OnMotorCollisionStay2D += OnMotorCollisionStay2D;
        }

        private void ReadInput(float timeStep)
        {
            // Update input buffer
            this.inputBuffer.Input = new Vector2(this.inputDriver.Horizontal, this.inputDriver.Vertical);

            this.inputBuffer.IsJumpPressed = this.inputDriver.Jump;

            if (this.inputBuffer.IsJumpPressed) {
                this.willJumpPaddingFrame = CalculateFramesFromTime(this.jumpSettings.WillJumpPaddingTime, timeStep);
            }

            this.inputBuffer.IsJumpHeld = this.inputDriver.HoldingJump;
            this.inputBuffer.IsJumpReleased = this.inputDriver.ReleaseJump;

            this.inputBuffer.IsDashPressed = this.inputDriver.Dash;
            this.inputBuffer.IsDashHeld = this.inputDriver.HoldingDash;
        }

        public void RunControllerCycle(float timeStep)
        {
            this.currentTimeStep = timeStep;

            UpdateTimers(timeStep);

            UpdateState(timeStep);

            // check padding frame
            if (this.willJumpPaddingFrame >= 0) {
                this.inputBuffer.IsJumpPressed = true;
            }

            // read input from input driver
            Vector2 input = this.inputBuffer.Input;

            Vector2Int rawInput = Vector2Int.zero;

            if (input.x > 0.0f)
                rawInput.x = 1;
            else if (input.x < 0.0f)
                rawInput.x = -1;

            if (input.y > 0.0f)
                rawInput.y = 1;
            else if (input.y < 0.0f)
                rawInput.y = -1;

            // check which side of character is collided
            int wallDirX = 0;

            if (this.motor.Collisions.Right) {
                wallDirX = 1;
            }
            else if (this.motor.Collisions.Left) {
                wallDirX = -1;
            }

            this.wallInteractionSettings.WallDirX = wallDirX;

            // check if want dashing
            if (this.inputBuffer.IsDashPressed) {
                StartDash(rawInput.x, timeStep);
            }

            // check if want climbing ladder
            if (IsInLadderArea()) {
                if (IsInLadderTopArea()) {
                    if (rawInput.y < 0) {
                        EnterLadderState();
                    }
                }
                else {
                    if (rawInput.y > 0) {
                        EnterLadderState();
                    }
                }
            }

            // dashing state
            if (IsState(MotorState.Dashing)) {
                this.velocity.x = this.dashModule.DashDir * this.dashModule.GetDashSpeed(); //getDashSpeed();

                if (!IsOnGround() && DashModule.UseGravity)
                    this.velocity.y = 0;

                if (DashModule.ChangeFacing) {
                    FacingDirection = (int)Mathf.Sign(this.velocity.x);
                }

                if (DashModule.UseCollision) {
                    this.motor.Move(this.velocity * timeStep, false);
                }
                // teleport, if there is no obstacle on the target position -> teleport, or use collision to find the closest teleport position
                else {
                    bool cannotTeleportTo = Physics2D.OverlapBox(
                        this.motor.Collider2D.bounds.center + this.velocity * timeStep,
                        this.motor.Collider2D.bounds.size,
                        0.0f,
                        this.motor.Raycaster.CollisionLayer);

                    if (!cannotTeleportTo) {
                        this.motor.transform.Translate(this.velocity * timeStep);
                    }
                    else {
                        this.motor.Move(this.velocity * timeStep, false);
                    }
                }
            }

            // on custom action
            else if (IsState(MotorState.CustomAction)) {
                //m_Velocity.x = m_ActionState.GetActionVelocity();

                //if (!IsGrounded() && DashModule.UseGravity)
                //    m_Velocity.y = 0;
            }

            // on ladder state
            else if (IsState(MotorState.OnLadder)) {
                this.velocity = input * this.ladderInteractionSettings.OnLadderSpeed;

                // jump if jump input is true
                if (this.inputBuffer.IsJumpPressed) {
                    StartJump(rawInput, wallDirX);
                }

                if (this.ladderInteractionSettings.LockFacingToRight) {
                    FacingDirection = 1;
                }
                else {
                    if (this.velocity.x != 0.0f)
                        FacingDirection = (int)Mathf.Sign(this.velocity.x);
                }

                //m_Motor.Move(m_Velocity * timeStep, false);

                // dont do collision detection
                if (this.onLadderState.HasRestrictedArea) {
                    // outside right, moving right disallowed
                    if (this.motor.transform.position.x > this.onLadderState.RestrictedAreaTopRight.x) {
                        if (this.velocity.x > 0.0f) {
                            this.velocity.x = 0.0f;
                        }
                    }

                    // outside left, moving left disallowed
                    if (this.motor.transform.position.x < this.onLadderState.RestrictedAreaBottomLeft.x) {
                        if (this.velocity.x < 0.0f) {
                            this.velocity.x = 0.0f;
                        }
                    }

                    // outside up, moving up disallowed
                    if (this.motor.transform.position.y > this.onLadderState.RestrictedAreaTopRight.y) {
                        if (this.velocity.y > 0.0f) {
                            this.velocity.y = 0.0f;
                        }
                    }

                    // outside down, moving down disallowed
                    if (this.motor.transform.position.y < this.onLadderState.RestrictedAreaBottomLeft.y) {
                        if (this.velocity.y < 0.0f) {
                            this.velocity.y = 0.0f;
                        }
                    }
                }

                Vector2 targetPos = this.motor.transform.position + this.velocity * timeStep;

                Vector2 currPos = this.motor.transform.position;

                // call Motor.Move to update collision info
                this.motor.Move(targetPos - currPos);

                // actual updated position
                this.motor.transform.position = targetPos;

                // Second pass check
                if (this.onLadderState.HasRestrictedArea) {
                    targetPos.x = Mathf.Clamp(targetPos.x, this.onLadderState.RestrictedAreaBottomLeft.x,
                        this.onLadderState.RestrictedAreaTopRight.x);
                    targetPos.y = Mathf.Clamp(targetPos.y, this.onLadderState.RestrictedAreaBottomLeft.y,
                        this.onLadderState.RestrictedAreaTopRight.y);

                    // restricted in x axis
                    if (targetPos.x != this.motor.transform.position.x) {
                        if (!this.ladderInteractionSettings.SnapToRestrictedArea) {
                            targetPos.x = Mathf.Lerp(this.motor.transform.position.x, targetPos.x, 0.25f);
                        }
                    }

                    // restricted in y axis
                    if (targetPos.y != this.motor.transform.position.y) {
                        if (!this.ladderInteractionSettings.SnapToRestrictedArea) {
                            targetPos.y = Mathf.Lerp(this.motor.transform.position.y, targetPos.y, 0.25f);
                        }
                    }

                    this.motor.transform.position = targetPos;
                }
            }
            else if (IsState(MotorState.Frozen)) {
                // Reset gravity if collision happened in y axis
                if (this.motor.Collisions.Above) {
                    //Debug.Log("Reset Vec Y");
                    this.velocity.y = 0;
                }
                else if (this.motor.Collisions.Below) {
                    // falling downward
                    if (this.velocity.y < 0.0f) {
                        this.velocity.y = 0;
                    }
                }

                if (this.applyGravity) {
                    float gravity = this.gravity;
                    this.velocity.y += gravity * timeStep;
                }

                this.motor.Move(this.velocity * timeStep, false);
            }
            else // other state
            {
                // fall through one way platform
                if (this.inputBuffer.IsJumpHeld && rawInput.y < 0) {
                    this.motor.FallThrough();
                    ChangeState(MotorState.Falling);
                }

                // setup velocity.x based on input
                float targetVecX = input.x * this.movementSettings.Speed;

                // smooth x direction motion
                if (IsOnGround()) {
                    this.velocity.x = targetVecX;
                    this.velocityXSmoothing = targetVecX;
                }
                else {
                    this.velocity.x = Mathf.SmoothDamp(this.velocity.x, targetVecX, ref this.velocityXSmoothing,
                        this.movementSettings.AccelerationTimeAirborne);
                }
                /*
            m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, targetVecX, ref m_VelocityXSmoothing,
                (m_Motor.Collisions.Below) ? m_MovementSettings.AccelerationTimeGrounded : m_MovementSettings.AccelerationTimeAirborne);
            */

                // check wall sticking and jumping
                bool isStickToWall = false;
                bool isGrabbingLedge = false;
                Vector2 ledgePos = Vector2.zero;

                if (IsAgainstWall()) {
                    // ledge grabbing logic
                    if (this.wallInteractionSettings.CanGrabLedge) {
                        if (CheckIfAtLedge(wallDirX, ref ledgePos)) {
                            if (!IsState(MotorState.OnLedge)) {
                                if (this.velocity.y < 0 && wallDirX == rawInput.x) {
                                    isGrabbingLedge = true;
                                    this.velocity.y = 0;

                                    float adjustY = ledgePos.y - MotorCollider.bounds.max.y;

                                    this.motor.transform.position += Vector3.up * adjustY;
                                }
                            }
                            else {
                                isGrabbingLedge = true;
                            }
                        }

                        if (isGrabbingLedge) {
                            ChangeState(MotorState.OnLedge);

                            this.airJumpCounter = 0;
                            this.OnResetJumpCounter.Invoke(MotorState.OnLedge);

                            // check if still sticking to wall
                            if (this.wallInteractionSettings.WallStickTimer > 0.0f) {
                                this.velocityXSmoothing = 0;
                                this.velocity.x = 0;

                                // leaving wall
                                if (rawInput.x == -wallDirX) {
                                    this.wallInteractionSettings.WallStickTimer -= timeStep;
                                }
                                // not leaving wall
                                else {
                                    this.wallInteractionSettings.WallStickTimer = this.wallInteractionSettings.WallStickTime;
                                }
                            }
                            else {
                                ChangeState(MotorState.Falling);
                                this.wallInteractionSettings.WallStickTimer = this.wallInteractionSettings.WallStickTime;
                            }
                        }
                    }

                    // wall sliding logic
                    if (!isGrabbingLedge && this.wallInteractionSettings.CanWallSlide) {
                        // is OnGround, press against wall and jump
                        if (IsOnGround()) {
                            if (!IsState(MotorState.WallSliding)) {
                                if (this.wallInteractionSettings.CanWallClimb) {
                                    if (rawInput.x == wallDirX && this.inputBuffer.IsJumpPressed) {
                                        isStickToWall = true;

                                        ConsumeJumpPressed();
                                    }
                                }
                            }
                        }
                        // is not OnGround, press against wall or was wallsliding
                        else {
                            if (IsState(MotorState.WallSliding) || rawInput.x == wallDirX) {
                                isStickToWall = true;
                            }
                        }

                        if (isStickToWall) {
                            ChangeState(MotorState.WallSliding);

                            this.airJumpCounter = 0;
                            this.OnResetJumpCounter.Invoke(MotorState.WallSliding);

                            // check if still sticking to wall
                            if (this.wallInteractionSettings.WallStickTimer > 0.0f) {
                                this.velocityXSmoothing = 0;
                                this.velocity.x = 0;

                                if (rawInput.x != wallDirX && rawInput.x != 0) {
                                    this.wallInteractionSettings.WallStickTimer -= timeStep;

                                    if (this.wallInteractionSettings.WallStickTimer < 0.0f) {
                                        ChangeState(MotorState.Falling);
                                        this.wallInteractionSettings.WallStickTimer = this.wallInteractionSettings.WallStickTime;
                                    }
                                }
                                else {
                                    this.wallInteractionSettings.WallStickTimer = this.wallInteractionSettings.WallStickTime;
                                }
                            }
                            else {
                                ChangeState(MotorState.Falling);
                                this.wallInteractionSettings.WallStickTimer = this.wallInteractionSettings.WallStickTime;
                            }
                        }
                    }
                }

                // Reset gravity if collision happened in y axis
                if (this.motor.Collisions.Above) {
                    //Debug.Log("Reset Vec Y");
                    this.velocity.y = 0;
                }
                else if (this.motor.Collisions.Below) {
                    // falling downward
                    if (this.velocity.y < 0.0f) {
                        this.velocity.y = 0;
                    }
                }

                // jump if jump input is true
                if (this.inputBuffer.IsJumpPressed && rawInput.y >= 0) {
                    StartJump(rawInput, wallDirX);
                }

                // variable jump height based on user input
                if (this.jumpSettings.HasVariableJumpHeight) {
                    if (!this.inputBuffer.IsJumpHeld && rawInput.y >= 0) {
                        if (this.velocity.y > this.minJumpSpeed)
                            this.velocity.y = this.minJumpSpeed;
                    }
                }

                if (this.applyGravity) {
                    float gravity = this.gravity;
                    if (IsState(MotorState.WallSliding) && this.velocity.y < 0) {
                        gravity *= this.wallInteractionSettings.WallSlideSpeedLoss;
                    }

                    this.velocity.y += gravity * timeStep;
                }

                // control ledge grabbing speed
                if (isGrabbingLedge) {
                    if (IsState(MotorState.OnLedge)) {
                        if (this.velocity.y < 0) {
                            this.velocity.y = 0;
                        }
                    }

                    FacingDirection = (this.motor.Collisions.Right) ? 1 : -1;
                }
                // control wall sliding speed
                else if (isStickToWall) {
                    if (this.wallInteractionSettings.CanWallClimb) {
                        if (IsState(MotorState.WallSliding)) {
                            this.velocity.y = input.y * this.wallInteractionSettings.WallClimbSpeed;
                        }
                    }
                    else {
                        if (this.velocity.y < -this.wallInteractionSettings.WallSlidingSpeedMax) {
                            this.velocity.y = -this.wallInteractionSettings.WallSlidingSpeedMax;
                        }
                    }

                    FacingDirection = (this.motor.Collisions.Right) ? 1 : -1;
                }
                else {
                    if (this.velocity.x != 0.0f)
                        FacingDirection = (int)Mathf.Sign(this.velocity.x);
                }

                this.motor.Move(this.velocity * timeStep, false);
            }

            // check ladder area
            if (IsInLadderArea()) {
                if (this.onLadderState.BottomArea.Contains(this.motor.Collider2D.bounds.center)) {
                    this.onLadderState.AreaZone = LadderZone.Bottom;
                }
                else if (this.onLadderState.TopArea.Contains(this.motor.Collider2D.bounds.center)) {
                    this.onLadderState.AreaZone = LadderZone.Top;
                }
                else if (this.onLadderState.Area.Contains(this.motor.Collider2D.bounds.center)) {
                    this.onLadderState.AreaZone = LadderZone.Middle;
                }
            }
        }

        private void UpdateTimers(float timeStep)
        {
            if (IsState(MotorState.Dashing)) {
                this.dashModule.CustomUpdate(timeStep);
            }

            if (IsState(MotorState.CustomAction)) {
                this.customActionModule.CustomUpdate(timeStep);
            }

            if (this.fallingJumpPaddingFrame >= 0) {
                this.fallingJumpPaddingFrame--;
            }

            if (this.willJumpPaddingFrame >= 0) {
                this.willJumpPaddingFrame--;
            }
        }

        private void UpdateState(float timeStep)
        {
            if (IsState(MotorState.Dashing)) {
                this.OnDashStay(this.dashModule.GetDashProgress());

                if (this.dashModule.GetDashProgress() >= 1.0f) {
                    EndDash();
                }
            }

            if (IsState(MotorState.Dashing)) {
                return;
            }

            if (IsState(MotorState.CustomAction)) {
                this.OnActionStay(this.customActionModule.GetActionProgress());

                if (this.customActionModule.GetActionProgress() >= 1.0f) {
                    EndAction();
                }
            }

            if (IsState(MotorState.CustomAction)) {
                return;
            }

            if (IsState(MotorState.Jumping)) {
                if (this.motor.Velocity.y < 0) {
                    EndJump();
                }
            }

            if (IsOnGround()) {
                if (IsState(MotorState.OnLadder)) {
                    if (this.ladderInteractionSettings.ExitLadderOnGround) {
                        if (!IsInLadderTopArea()) {
                            ChangeState(MotorState.OnGround);
                        }
                    }
                }
                else {
                    if (!IsState(MotorState.Frozen)) {
                        ChangeState(MotorState.OnGround);
                    }
                }
            }
            else {
                if (IsState(MotorState.OnGround)) {
                    this.fallingJumpPaddingFrame = CalculateFramesFromTime(this.jumpSettings.FallingJumpPaddingTime, timeStep);

                    ChangeState(MotorState.Falling);
                }
            }

            if (IsState(MotorState.WallSliding)) {
                if (!IsAgainstWall()) {
                    if (this.motor.Velocity.y < 0.0f) {
                        ChangeState(MotorState.Falling);
                    }
                    else {
                        ChangeState(MotorState.Jumping);
                    }
                }
            }
        }

        private void StartJump(Vector2Int rawInput, int wallDirX)
        {
            bool success = false;

            if (IsState(MotorState.OnLedge)) {
                StartLedgeJump();
                success = true;
            }
            else if (IsState(MotorState.WallSliding)) {
                if (this.wallInteractionSettings.CanWallJump) {
                    StartWallJump(rawInput.x, wallDirX);
                    success = true;
                }
            }
            else {
                success = StartNormalJump();
            }

            if (success) {
                ConsumeJumpPressed();

                ChangeState(MotorState.Jumping);

                this.OnJump.Invoke();
            }
        }

        private bool StartNormalJump()
        {
            if (IsState(MotorState.OnGround)) {
                this.velocity.y = this.maxJumpSpeed;

                this.OnNormalJump.Invoke();

                return true;
            }
            else if (IsState(MotorState.OnLadder)) {
                this.velocity.y = this.jumpSettings.OnLadderJumpForce;

                this.OnLadderJump.Invoke();

                return true;
            }
            else if (this.fallingJumpPaddingFrame >= 0) {
                this.velocity.y = this.maxJumpSpeed;

                this.OnNormalJump.Invoke();

                return true;
            }
            else if (CanAirJump()) {
                this.velocity.y = this.maxJumpSpeed;
                this.airJumpCounter++;

                this.OnAirJump.Invoke();

                return true;
            }
            else {
                return false;
            }
        }

        private void StartLedgeJump()
        {
            this.velocity.y = this.maxJumpSpeed;

            this.OnLedgeJump.Invoke();
        }

        private void StartWallJump(int rawInputX, int wallDirX)
        {
            bool climbing = wallDirX == rawInputX;
            Vector2 jumpVec;

            // climbing
            if (climbing || rawInputX == 0) {
                jumpVec.x = -wallDirX * this.wallInteractionSettings.ClimbForce.x;
                jumpVec.y = this.wallInteractionSettings.ClimbForce.y;
            }
            // jump leap
            else {
                jumpVec.x = -wallDirX * this.wallInteractionSettings.LeapForce.x;
                jumpVec.y = this.wallInteractionSettings.LeapForce.y;
            }

            this.WallJumpExecuted.Invoke(jumpVec);

            this.velocity = jumpVec;
        }

        private void ConsumeJumpPressed()
        {
            this.inputBuffer.IsJumpPressed = false;
            this.fallingJumpPaddingFrame = -1;
            this.willJumpPaddingFrame = -1;
        }

        // highest point reached, start falling
        private void EndJump()
        {
            if (IsState(MotorState.Jumping)) {
                ChangeState(MotorState.Falling);
            }
        }

        private void StartDash(int rawInputX, float timeStep)
        {
            if (DashModule == null) {
                return;
            }

            if (IsState(MotorState.Dashing)) {
                return;
            }

            if (DashModule.CanOnlyBeUsedOnGround) {
                if (!IsOnGround()) {
                    return;
                }
            }

            int dashDir = (rawInputX != 0) ? rawInputX : FacingDirection;
            if (!DashModule.CanDashToSlidingWall) {
                if (IsState(MotorState.WallSliding)) {
                    int wallDir = (this.motor.Collisions.Right) ? 1 : -1;
                    if (dashDir == wallDir) {
                        //Debug.Log("Dash Disallowed");
                        return;
                    }
                }
            }

            if (DashModule.ChangeFacing) {
                FacingDirection = (int)Mathf.Sign(this.velocity.x);
            }

            if (!IsOnGround() && DashModule.UseGravity)
                this.velocity.y = 0;

            this.dashModule.Start(dashDir, timeStep);

            this.OnDash.Invoke(dashDir);

            ChangeState(MotorState.Dashing);
        }

        private void EndDash()
        {
            if (IsState(MotorState.Dashing)) {
                // smooth out or sudden stop
                float vecX = this.dashModule.DashDir * this.dashModule.GetDashSpeed();
                this.velocityXSmoothing = vecX;
                this.velocity.x = vecX;
                ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
            }
        }

        private void StartAction(int rawInputX)
        {
            if (ActionModule == null) {
                return;
            }

            if (IsState(MotorState.CustomAction)) {
                return;
            }

            if (DashModule.CanOnlyBeUsedOnGround) {
                if (!IsOnGround()) {
                    return;
                }
            }

            int actionDir = (rawInputX != 0) ? rawInputX : FacingDirection;

            if (!ActionModule.CanUseToSlidingWall) {
                if (IsState(MotorState.WallSliding)) {
                    int wallDir = (this.motor.Collisions.Right) ? 1 : -1;
                    if (actionDir == wallDir) {
                        return;
                    }
                }
            }

            this.customActionModule.Start(actionDir);

            ChangeState(MotorState.CustomAction);
        }

        private void EndAction()
        {
            if (IsState(MotorState.CustomAction)) {
                // smooth out or sudden stop
                Vector2 vec = new Vector2(this.customActionModule.ActionDir, 1) * this.customActionModule.GetActionVelocity();
                this.velocityXSmoothing = vec.x;
                this.velocity = vec;
                ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
            }
        }

        private void EnterLadderState()
        {
            this.velocity.x = 0;
            this.velocity.y = 0;

            ChangeState(MotorState.OnLadder);

            this.airJumpCounter = 0;
            this.OnResetJumpCounter.Invoke(MotorState.OnLadder);
            this.applyGravity = false;
        }

        private void ExitLadderState()
        {
            this.velocity.y = 0;
            ChangeState(IsOnGround() ? MotorState.OnGround : MotorState.Falling);
        }

        // change state and callback
        private void ChangeState(MotorState state)
        {
            if (this.motorState == state) {
                return;
            }

            if (IsState(MotorState.OnLadder)) {
                this.applyGravity = true;
            }

            // exit old state action
            if (IsState(MotorState.Jumping)) {
                this.OnJumpEnd.Invoke();
            }

            if (IsState(MotorState.Dashing)) {
                this.OnDashEnd.Invoke();
            }

            if (IsState(MotorState.WallSliding)) {
                this.WallSlided.Invoke();
            }

            if (IsState(MotorState.OnLedge)) {
                this.OnLedgeGrabbingEnd.Invoke();
            }

            // set new state
            var prevState = this.motorState;
            this.motorState = state;

            if (IsState(MotorState.OnGround)) {
                this.airJumpCounter = 0;
                this.OnResetJumpCounter.Invoke(MotorState.OnGround);

                if (prevState != MotorState.Frozen) {
                    this.OnLanded.Invoke();
                }
            }

            if (IsState(MotorState.OnLedge)) {
                this.OnLedgeGrabbing.Invoke(this.wallInteractionSettings.WallDirX);
            }

            if (IsState(MotorState.WallSliding)) {
                this.WallSliding.Invoke(this.wallInteractionSettings.WallDirX);
            }

            this.OnMotorStateChanged.Invoke(prevState, this.motorState);
        }

        private int CalculateFramesFromTime(float time, float timeStep)
        {
            return Mathf.RoundToInt(time / timeStep);
        }

        private void OnMotorCollisionEnter2D(MotorCollision2D col)
        {
            if (col.IsSurface(MotorCollision2D.CollisionSurface.Ground)) {
                OnCollisionEnterGround();
            }

            if (col.IsSurface(MotorCollision2D.CollisionSurface.Ceiling)) {
                OnCollisionEnterCeiling();
            }

            if (col.IsSurface(MotorCollision2D.CollisionSurface.Left)) {
                OnCollisionEnterLeft();
            }

            if (col.IsSurface(MotorCollision2D.CollisionSurface.Right)) {
                OnCollisionEnterRight();
            }
        }

        private void OnCollisionEnterGround()
        {
            //Debug.Log("Ground!");
        }

        private void OnCollisionEnterCeiling()
        {
            //Debug.Log("Ceiliing!");
        }

        private void OnCollisionEnterLeft()
        {
            //Debug.Log("Left!");
        }

        private void OnCollisionEnterRight()
        {
            //Debug.Log("Right!");
        }

        private void OnMotorCollisionStay2D(MotorCollision2D col)
        {
            if (col.IsSurface(MotorCollision2D.CollisionSurface.Ground)) {
                OnCollisionStayGround();
            }

            if (col.IsSurface(MotorCollision2D.CollisionSurface.Left)) {
                OnCollisionStayLeft();
            }

            if (col.IsSurface(MotorCollision2D.CollisionSurface.Right)) {
                OnCollisionStayRight();
            }
        }

        private void OnCollisionStayGround()
        {
            //Debug.Log("Ground!");
            //m_AirJumpCounter = 0;
        }

        private void OnCollisionStayLeft()
        {
            //Debug.Log("Left!");
            //if (m_WallJumpSettings.CanWallJump)
            //{
            //    m_AirJumpCounter = 0;
            //}
        }

        private void OnCollisionStayRight()
        {
            //Debug.Log("Right!");
            //if (m_WallJumpSettings.CanWallJump)
            //{
            //    m_AirJumpCounter = 0;
            //}
        }
    }
}