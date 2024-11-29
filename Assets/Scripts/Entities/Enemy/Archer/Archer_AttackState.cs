﻿using System.Collections;
using UnityEngine;



    public class Archer_AttackState : EnemyState
    {

    private Enemy_Archer enemy;
        public Archer_AttackState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
        {
        this.enemy = _enemy;
        }

    public override void Enter()
    {
        base.Enter();
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
            stateMachine.ChangeState(enemy.battleState);
    }   
}