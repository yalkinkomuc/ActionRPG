using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeletonn enemy;
    private int movedirection;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeletonn _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if(player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
        
    }

    public override void Update()
    {



        if(enemy.isPlayerDetected())
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
            if (stateTimer < 0||Vector2.Distance(player.transform.position,enemy.transform.position)<15)
                stateMachine.ChangeState(enemy.idleState);
        }

        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if(distanceToPlayerX < .8f)
            return;

        base.Update();
        if (player.position.x > enemy.transform.position.x)
            movedirection = 1;
        else if (player.position.x < enemy.transform.position.x)
            movedirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * movedirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked+enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.minAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
    
}
