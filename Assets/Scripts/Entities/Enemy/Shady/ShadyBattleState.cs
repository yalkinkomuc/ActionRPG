using UnityEngine;


public class ShadyBattleState : EnemyState
{
    private Transform player;
    private Enemy_Shady enemy;
    private int movedirection;

    private float defaultSpeed;
    public ShadyBattleState(global::Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;

    }
    public override void Enter()
    {
        base.Enter();
        defaultSpeed = enemy.moveSpeed;

        enemy.moveSpeed = enemy.battleStateMoveSpeed;

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
            enemy.stats.killEntity();
               
            


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

        enemy.SetVelocity(enemy.moveSpeed * movedirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed = defaultSpeed;
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
