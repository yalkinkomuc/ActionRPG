using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum StatType
{

    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;


    [Header("Major Stats")]
    public Stats Strength; //1 Puan hasarı 1 arttırır. ve Crit gücünü %1 arttırır.
    public Stats Agility; // 1 Puan Evasionu 1 arttırır ve crit şansını %1 arttırır.
    public Stats Intelligence;//1 Puan büyü hasarını 1 arttırır. ve büyücü direncini 1 yükseltir.
    public Stats Vitality; //1 puan arttırmak canı 5 arttırır.


    [Header("Offensive Stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;// default 150%


    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats Armor;
    public Stats Evasion;
    public Stats magicResistance;


    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;


    public bool isIgnited; // zamanla hasar ver
    public bool isChilled; // rakibin zırhını %20 düşürür
    public bool isShocked; // vuruş isabetini %20 düşürür


    [SerializeField] private float ailmentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;



    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public bool  isDead {  get; private set; }
    public bool isVulnerable { get; private set; }

    public bool isInvincible { get; private set; }

    public int currentHealth;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        currentHealth = maxHealth.getValue();
        critPower.SetDefaultValue(150);

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;


        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (chilledTimer < 0)
            isChilled = false;


        if(isIgnited)
        ApplyIgniteDamage();
    }

    public void makeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCoroutine(_duration));
    }


    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }


    public virtual void IncreaseStatBy(int _modifier, float _duration,Stats _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

     private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
   

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;    

        if(_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().setupKnockbackDirection(transform);

        int totalDamage = damage.getValue() + Strength.getValue();

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            criticalStrike = true;
        }

        fx.createHitFX(_targetStats.transform,criticalStrike);

        totalDamage = CheckTargetsArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats); // düz atakta magic damage istemiyorsan kaldır.
    }

    



    #region Apply Ailments and Magical Damage
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.getValue();
        int _iceDamage = iceDamage.getValue();
        int _lightingDamage = lightingDamage.getValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + Intelligence.getValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttempToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private  void AttempToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {

            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (Random.value < .5f && _lightingDamage > 0)
        {
            canApplyShock = true;
            _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
            return;
        }


        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));



        if (canApplyShock)
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite&&canApplyIgnite)
        {

            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.igniteFxFor(ailmentsDuration);
        }

        if(_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if(_shock && canApplyShock)
        {

            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {

                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }

        }
       
        

    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;


        shockedTimer = ailmentsDuration;
        isShocked = _shock;
        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());

        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
   public void SetupShockDamage(int _damage) => shockDamage = _damage;
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0 )
        {

            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0&& !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }


    #endregion

    #region Stat Calculations
    protected int CheckTargetsArmor(CharacterStats _targetStats, int totalDamage)
    {

        if(_targetStats.isChilled)
        totalDamage -= Mathf.RoundToInt(_targetStats.Armor.getValue()*.8f); 
        else
            totalDamage -= _targetStats.Armor.getValue();


        totalDamage -= _targetStats.Armor.getValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void onEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.Evasion.getValue() + _targetStats.Agility.getValue();

        if (isShocked)
            totalEvasion += 20;


        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.onEvasion();
            return true;
        }
        return false;
    }

    

    public virtual void TakeDamage(int _damage)
    {
        if(isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFx");

        if (currentHealth < 0&& !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > getMaxHealthValue())
            currentHealth = getMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);


        currentHealth -= _damage;

        if (_damage > 0)
            fx.createPopUpText(_damage.ToString());

        if (onHealthChanged != null)
            onHealthChanged();
    }



    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.getValue()+Agility.getValue();

        if(Random.Range(0, 100) <= totalCriticalChance) 
        {
        return true;
        }

        return false;
    }

    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.getValue()+Strength.getValue())*.01f;       
        float critDamage = _damage*totalCritPower;
        
        return Mathf.RoundToInt(critDamage);
    }

    public int getMaxHealthValue()
    {
        return maxHealth.getValue() + Vitality.getValue() * 5;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.getValue() + (_targetStats.Intelligence.getValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }


    #endregion

    protected virtual void Die()
    {
        isDead = true;
    }

    public void killEntity ()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible (bool _isInvincible) => isInvincible = _isInvincible;
    public Stats GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return Strength;
        else if (_statType == StatType.agility) return Agility;
        else if (_statType == StatType.intelligence) return Intelligence;
        else if (_statType == StatType.vitality) return Vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.armor) return Armor;
        else if (_statType == StatType.magicRes) return magicResistance;
        else if (_statType == StatType.evasion) return Evasion;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightingDamage) return lightingDamage;

        return null;


    }
}
