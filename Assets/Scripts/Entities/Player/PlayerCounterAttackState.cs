
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccesfulCounterAttack",false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                SuccesfullCounterAttack();
                hit.GetComponent<Arrow_Controller>().flipArrow();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        SuccesfullCounterAttack();

                        player.skill.parry.UseSkill();

                        if (canCreateClone)
                        {
                            canCreateClone = false;
                            player.skill.parry.MakeMirageOnParry(hit.transform);
                        }
                    }
                }
        }
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void SuccesfullCounterAttack()
    {
        stateTimer = 10; // 1 den büyük herhangi bir değer
        player.anim.SetBool("SuccesfulCounterAttack", true);
    }
}
