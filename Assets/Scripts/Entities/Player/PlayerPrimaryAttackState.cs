using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    public int comboCounter { get; private set; }
    private float lasttimeattacked;
    private float comboWindow = 2;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        

        if (comboCounter > 2||Time.time >=lasttimeattacked+comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingdir;
        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;        
    }





    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lasttimeattacked = Time.time;       
        player.StartCoroutine("BusyFor", .15f);
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if(stateTimer<0)
player.ZeroVelocity();
    }
}
