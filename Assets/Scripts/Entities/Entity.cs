using System.Collections;
using UnityEngine;


public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    

    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockBackPower = new Vector2(7, 12);
    [SerializeField] protected Vector2 knockBackOffset = new Vector2(0.5f, 2);
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;

    [Header("CollisionInfo")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.19f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 0.8f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 1.31f;
    [SerializeField] protected LayerMask whatisGround;


    public int facingdir { get; private set; } = 1;
    protected bool facingright = true;
    public int knockBackDirection { get; private set; }


    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    public virtual void setupKnockbackDirection(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockBackDirection = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockBackDirection = 1;
    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    #region Velocity
    public void ZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }




    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatisGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingdir, wallCheckDistance, whatisGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingdir, wallCheck.position.y));
        Gizmos.DrawSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip

    public virtual void Flip()
    {
        facingdir = facingdir * -1;
        facingright = !facingright;
        transform.Rotate(0, 180, 0);


        if (onFlipped != null)
            onFlipped();
    }
    #endregion


    #region Damage
    public virtual void DamageImpact() => StartCoroutine("HitKnockBack");

    #endregion

    #region HitKnockBack
    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        float xOffset = Random.Range(knockBackOffset.x, knockBackOffset.y);

        if(knockBackPower.x> 0 || knockBackPower.y> 0)
            rb.velocity = new Vector2((knockBackPower.x + xOffset) * knockBackDirection, knockBackPower.y);


        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnocback();
    }
    protected virtual void SetupZeroKnocback()
    {

    }

    public void SetupKnockBackPower(Vector2 _knockBackPower) => knockBackPower = _knockBackPower;
    #endregion

    #region FlipController
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingright)
            Flip();
        else if (_x < 0 && facingright)
            Flip();
    }

    public virtual void SetupDefaultFacingDir (int _direction)
    {
        facingdir = _direction;

        if (facingdir == -1)
            facingright = false;
    }


    #endregion


    public virtual void Die()
    {

    }
}

