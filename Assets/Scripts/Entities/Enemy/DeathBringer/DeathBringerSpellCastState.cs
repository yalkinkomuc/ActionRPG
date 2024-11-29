using UnityEngine;


public class DeathBringerSpellCastState : EnemyState
{
    private Enemy_DeathBringer enemy;

    private int amountOfSpells;
    private float spelltimer;

    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells = enemy.amountOfSpells;
        spelltimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        spelltimer -= Time.deltaTime;

        if (canCast())
            enemy.castSpell();

        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    private bool canCast()
    {
        if (amountOfSpells > 0 && spelltimer < 0)
        {
            amountOfSpells--;
            spelltimer = enemy.spellCoolDown;
            return true;
        }
        return false;


    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }
}
