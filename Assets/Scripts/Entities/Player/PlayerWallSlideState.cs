using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.IsWallDetected())
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        if(player.IsWallDetected() == false)
            stateMachine.ChangeState(player.airState);

        base.Update();

        if (player.input.jumpPressed)
        {
            stateMachine.ChangeState(player.wallJumpState);
            rb.velocity = new Vector2(xInput*5, player.JumpForce);
            player.Flip();
            return;
        }

        if (xInput != 1 && player.facingdir != xInput &&xInput != -1)
            stateMachine.ChangeState(player.idleState);

        if (yInput < 0)
        rb.velocity = new Vector2(0,rb.velocity.y );
        else
        rb.velocity = new Vector2(0, rb.velocity.y * .7f);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        

    }
}
