using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats soulsDropAmount;   

    [Header("Level Details")]
    [SerializeField] private int Level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;
    protected override void Start()
    {
        //soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(Strength);
        Modify(Vitality);
        Modify(Agility);
        Modify(Intelligence);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(Armor);
        Modify(Evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);


        
    }

    private void Modify (Stats _stats)
    {
        for (int i = 1; i < Level; i++)
        {
            float modifier = _stats.getValue() * percentageModifier;

            _stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.getValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
