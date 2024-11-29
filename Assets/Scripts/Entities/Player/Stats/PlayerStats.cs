using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();

    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if(isDead)
            return;

        int randomSound = Random.Range(33, 34);

        if(_damage > getMaxHealthValue() * .3f)
        {
            player.SetupKnockBackPower(new Vector2(5,9));
            player.playerFX.ScreenShake(player.playerFX.shakeHighDamage);
            AudioManager.instance.PlaySfx(randomSound, null);
        }



        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
       
    }

    public override void onEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();

    }
    public void cloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.getValue() + Strength.getValue();

        if(_multiplier > 0) 
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);


        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }


        totalDamage = CheckTargetsArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats); // düz atakta magic damage istemiyorsan kaldır.
    }
}
