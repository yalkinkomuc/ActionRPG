
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon ,
    Armor,
    Amulet,
    Flask
}



[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique Effect")]

    public float ItemCooldown;
    public ItemEffect[] itemEffects;

    

    [Header("Major Stats")]
    public int strength;
    public int vitality;
    public int intelligence;
    public int agility;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    private int DescriptionLength;


    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect( _enemyPosition);
        }
    }
    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.Strength.AddModifier(strength);
        playerStats.Agility.AddModifier(agility);
        playerStats.Intelligence.AddModifier(intelligence);
        playerStats.Vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.Armor.AddModifier(armor);
        playerStats.Evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }
    
    public void RemoveModifier() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.Strength.RemoveModifier(strength);
        playerStats.Agility.RemoveModifier(agility);
        playerStats.Intelligence.RemoveModifier(intelligence);
        playerStats.Vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.Armor.RemoveModifier(armor);
        playerStats.Evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string getDescription()
    {
        sb.Length = 0;
        DescriptionLength = 0;


        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "CritPower");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resistance");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lightning Damage");


        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine("  Unique:  " + itemEffects[i].effectDescription);
                DescriptionLength++;
            }
        }




        if(DescriptionLength < 5)
        {
            for (int i = 0; i < 5 - DescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append(" ");
            }

           

        }


        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0 ) 
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (_value > 0)
                sb.Append(_name + " : " + _value);

            DescriptionLength++;

        }
    }
}
