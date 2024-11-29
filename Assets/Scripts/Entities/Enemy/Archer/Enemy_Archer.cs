using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer spesific info ")]

    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowDamage;


    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional collision Check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;


    #region States
    public Archer_IdleState idleState { get; private set; }
    public Archer_MoveState moveState { get; private set; }
    public Archer_AttackState attackState { get; private set; }
    public Archer_BattleState battleState { get; private set; }
    public Archer_StunnedState stunnedState { get; private set; }
    public Archer_DeadState deadState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new Archer_IdleState(this, stateMachine, "Idle", this);
        moveState = new Archer_MoveState(this, stateMachine, "Move", this);
        attackState = new Archer_AttackState(this, stateMachine, "Attack", this);
        battleState = new Archer_BattleState(this, stateMachine, "Idle", this);
        stunnedState = new Archer_StunnedState(this, stateMachine, "Stunned", this);
        deadState = new Archer_DeadState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initalize(moveState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrow, attackCheck.position, Quaternion.identity);

        newArrow.GetComponent<Arrow_Controller>().SetupArrow(arrowSpeed * facingdir, stats);
    }

    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatisGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingdir, wallCheckDistance + 2, whatisGround);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position,groundBehindCheckSize);
    }

}
