using UnityEngine;



public class Archer_BattleState : EnemyState
{
    private Transform player;
    private Enemy_Archer enemy;
    private int movedirection;

    public Archer_BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        base.Update();

        if (enemy.isPlayerDetected())
        {

            stateTimer = enemy.battletime;

            if (enemy.isPlayerDetected().distance < enemy.safeDistance)
            {
                if (canJump())
                    stateMachine.ChangeState(enemy.jumpState);
            }


            if (enemy.isPlayerDetected().distance < enemy.attackdistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }


        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipController();

        //enemy.SetVelocity(enemy.moveSpeed * movedirection, rb.velocity.y);
    }

    private void BattleStateFlipController()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingdir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingdir == 1)
            enemy.Flip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    private bool canJump()
    {

        if (enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true)
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
}
