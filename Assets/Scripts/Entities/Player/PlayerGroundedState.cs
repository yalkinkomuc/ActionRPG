using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if ((player.input.blackHolePressed)&&player.skill.blackHole.BlackHoleUnlocked)
        {
            if (player.skill.blackHole.cooldownTimer > 0)
            {
                player.playerFX.createPopUpText("Cooldown");
                return;
            }
                

            stateMachine.ChangeState(player.blackHoleState);
        }

        if ((Input.GetKeyDown(KeyCode.Mouse1))&&HasNoSword()&& player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordSate);

        if (player.input.counterAttackPressed && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (player.input.jumpPressed&&player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if (player.input.attackPressed)
            stateMachine.ChangeState(player.primaryAttackState);
    }
     private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;


    }
}
