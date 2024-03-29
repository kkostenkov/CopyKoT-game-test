using System;
using DYP;

public class ThiefAnimationController : AdventurerAnimationController
{
    private BaseMotor2D motor;

    protected override void Awake()
    {
        base.Awake();
        motor = GetComponent<BaseMotor2D>();
    }

    protected override void Update()
    {
        base.Update();
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
}