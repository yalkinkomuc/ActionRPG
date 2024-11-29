using System.Collections;
using UnityEngine;



    public class DeathBringerAttackState : EnemyState
    {
    private Enemy_DeathBringer enemy;

        public DeathBringerAttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 5;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();

        if (triggerCalled)
        {
            if(enemy.canTeleport())
            stateMachine.ChangeState(enemy.teleportState);
        else
                stateMachine.ChangeState(enemy.battleState);
        }
            

    }

}
