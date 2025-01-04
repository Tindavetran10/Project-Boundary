using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFailingState : PlayerAirborneState
{
    private PlayerFallData fallData;
    private Vector3 playerPositionOnEnter;
    
    public PlayerFailingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        fallData = airborneData.FallData;
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

        playerPositionOnEnter = stateMachine.Player.transform.position;

        stateMachine.ReusableData.MovementSpeedModifier = 0;

        ResetVerticalVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        LimitVerticalVerlocity();
    }
    #endregion

    #region Reusable Methods
    protected override void ResetSprintState()
    {
        
    }

    protected override void OnContactWithGround(Collider collider)
    {
        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.RunningState);

            return;
        }
        
        
        float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

        if (fallDistance < fallData.MinimumDistanceToBeConsidereHardFall)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);

            return;
        }

        if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.HardLandingState); 
            
            return;
        }

        stateMachine.ChangeState(stateMachine.RollingState);
    }
    #endregion


    #region Main Methods
    private void LimitVerticalVerlocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        if (playerVerticalVelocity.y >= -fallData.FallSpeedLimit)
        {
            return;
        }

        Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

        stateMachine.Player.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }
    #endregion
}
