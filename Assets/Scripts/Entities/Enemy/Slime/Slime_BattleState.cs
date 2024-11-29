using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime_BattleState : EnemyState
{
    private Enemy_Slime enemy;
    private Transform player;
    private int movedirection;
    public Slime_BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

    }

    public override void Update()
    {



        if (enemy.isPlayerDetected())
        {

            stateTimer = enemy.battletime;

            if (enemy.isPlayerDetected().distance < enemy.attackdistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }


        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) < 15)
                stateMachine.ChangeState(enemy.idleState);
        }


        base.Update();
        if (player.position.x > enemy.transform.position.x)
            movedirection = 1;
        else if (player.position.x < enemy.transform.position.x)
            movedirection = -1;

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackdistance - .5f)
            return;



        enemy.SetVelocity(enemy.moveSpeed * movedirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.minAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

}

