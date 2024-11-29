using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void animTrigger()
    {
        player.animationTrigger();
    }
    private void attackTrigger()
    {

        AudioManager.instance.PlaySfx(2,null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit  in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats  _target = hit.GetComponent<EnemyStats>();

                if (_target != null) 
                player.stats.DoDamage(_target);

                ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(_target.transform);



            }
        }

    }

   

    private void throwSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
