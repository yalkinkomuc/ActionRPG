using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big,medium,small}

public class Enemy_Slime : Enemy
{
    [Header("Slime Spesific")]
    [SerializeField] private SlimeType SlimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;




    #region States

    public Slime_IdleState idleState {  get; private set; }
    public Slime_MoveState moveState { get; private set; }
    public Slime_BattleState battleState { get; private set; }
    public Slime_AttackState attackState { get; private set; }
    public Slime_StunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new Slime_IdleState(this, stateMachine, "Idle",this);
        moveState = new Slime_MoveState(this, stateMachine,"Move",this);
        battleState = new Slime_BattleState(this, stateMachine, "Move", this);
        attackState = new Slime_AttackState(this, stateMachine, "Attack", this);
        stunnedState = new Slime_StunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initalize(idleState);
    }

    protected override void Update()
    {
        base.Update();
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

        if(SlimeType == SlimeType.small)
            return;

        //CreateSlime(slimesToCreate, slimePrefab);
    }

    private void CreateSlime(int _amountOfSlimes,GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingdir);
        }
    }

    public void SetupSlime(int _facingDir)
    {

        if (_facingDir != facingdir)
            Flip();


        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingdir, yVelocity);

        Invoke("CancelKnockBack", 1.5f);
    }

    private void CancelKnockBack() => isKnocked = false;

}
