using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttackState : PlayerState
{
   
    public PlayerDashAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

   

    
    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocity(player.facingdir*player.movespeed*.2f,rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
        
        player.SetVelocity(player.facingdir*player.movespeed*.2f,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    
}
