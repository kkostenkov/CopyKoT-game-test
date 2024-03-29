using System;
using DYP;
using UnityEngine;

public class ThiefAnimationController : MonoBehaviour
{
    private BaseMotor2D motor;
    private BaseMovementController m_Controller;

    [SerializeField]
    private Transform m_Appearance;

    public Transform Appearance { get { return m_Appearance; } }

    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    protected Animator m_Animtor;
    
    private void Awake()
    {
        m_Controller = GetComponent<BaseMovementController>();

        m_SpriteRenderer = m_Appearance.GetComponentInChildren<SpriteRenderer>();
        motor = GetComponent<BaseMotor2D>();
    }

    private void Start()
    {
        m_Controller.OnFacingFlip += onFacingFlip;
        m_Controller.OnDash += onDash;
        m_Controller.OnDashStay += onDashStay;
        m_Controller.OnDashEnd += onDashEnd;
        m_Controller.OnWallJump += onWallJump;

        m_Controller.OnWallSliding += onWallSliding;
        m_Controller.OnWallSlidingEnd += onWallSlidingEnd;
    }

    private void Update()
    {
        if (Mathf.Abs(m_Controller.InputVelocity.x) > 0.0f)
            m_Animtor.SetFloat("SpeedX", 1.0f);
        else
            m_Animtor.SetFloat("SpeedX", 0.0f);

        m_Animtor.SetFloat("SpeedY", m_Controller.InputVelocity.y);

        m_Animtor.SetBool("IsGrounded", m_Controller.IsOnGround());

        m_Animtor.SetBool("IsOnWall", m_Controller.IsState(MotorState.WallSliding));
        
        var motorVelocity = motor.Velocity;
        if (AlmostEquals(motorVelocity.x, 0, 0.01f)) {
            this.m_Animtor.SetFloat("SpeedX", 0.0f);
        }
        else {
            this.m_Animtor.SetFloat("SpeedX", 1.0f);
        }
    }

    public static bool AlmostEquals(float double1, float double2, float precision)
    {
        return (Math.Abs(double1 - double2) <= precision);
    }

    private void onFacingFlip(int facing)
    {
        m_Appearance.localScale = new Vector3(facing, 1, 1);
    }

    private void onWallJump(Vector2 vec)
    {
        // TODO
    }

    private void onWallSliding(int wallDir)
    {
        //m_SpriteRenderer.flipX = true;
    }

    private void onWallSlidingEnd()
    {
        //m_SpriteRenderer.flipX = false;
    }

    private void onDash(int dashDir)
    {
        m_Animtor.SetBool("IsDashing", true);
        m_Animtor.Play("StartDash");
    }

    private void onDashStay(float progress)
    {
        m_Animtor.SetFloat("DashProgress", progress);
    }

    private void onDashEnd()
    {
        m_Animtor.SetBool("IsDashing", false);
    }
}