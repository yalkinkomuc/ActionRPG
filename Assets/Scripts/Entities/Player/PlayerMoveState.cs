using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySfx(14,null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSfx(14);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput*player.movespeed, rb.velocity.y);
        if (xInput == 0 )
            stateMachine.ChangeState(player.idleState);
    }
}