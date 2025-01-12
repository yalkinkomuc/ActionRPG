using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/ItemEffect/Freeze Enemies")]

public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();


        if (playerStats.currentHealth >  playerStats.getMaxHealthValue() * .1f)
            return;

        if(!Inventory.instance.canUseArmor())
            return;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
           hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
