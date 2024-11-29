using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Entity
{


    #region Interfaces

    public IPlayerInput input { get; set; }

    #endregion
    
    
    
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy {  get; private set; }

    [Header("MoveInfo")]

    public float movespeed;
    public float JumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

    [Header("DashInfo")]

    public float dashSpeed;
    public float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashDir { get; private set; }

    [Header("Bind Timer")]
    public float bindTimer;
    public bool destroyedFirstBinding;

    public bool countDownActive = false;
    public float bindCooldown = 10;
    
    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }
    
    public PlayerFX playerFX { get; private set; }
   
   
    

    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordSate { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerDashAttackState dashAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {

        base.Awake();
        stateMachine = new PlayerStateMachine();
        input = new KeyboardInput();

        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordSate = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        dashAttackState = new PlayerDashAttackState(this, stateMachine, "DashAttack");
    }
    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        playerFX = GetComponent<PlayerFX>();
        defaultJumpForce = JumpForce;
        defaultMoveSpeed = movespeed;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        
        
        
        if (Time.timeScale == 0)
            return;

        if (countDownActive)
        {
        bindTimer -= Time.deltaTime;
        Debug.Log(bindTimer);
            
        }
        
        

        base .Update();

        stateMachine.currentState.Update();
        CheckforDashInput();

        if (input.crystalSkillPressed&& skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        if (input.useFlaskPressed)
            Inventory.instance.UseFlask();

    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        movespeed = movespeed * (1 - _slowPercentage);
        JumpForce = JumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1- _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        movespeed = defaultMoveSpeed;
        JumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;

    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

   

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);

    isBusy = false;
    }

    public void animationTrigger() => stateMachine.currentState.AnimationFinish();
    private void CheckforDashInput()
    {
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;
        

        if ((input.dashPressed) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingdir;

            stateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnocback()
    {
        knockBackPower = new Vector2(0, 0);
    }




}
