using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Stat_Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;


    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private void OnValidate()
    {
            gameObject.name = "Stat -"+ statName;

        if (statNameText != null )
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).getValue().ToString();



            if(statType == StatType.health) 
                statValueText.text = playerStats.getMaxHealthValue().ToString();
            
            if (statType == StatType.damage)    
                statValueText.text = (playerStats.damage.getValue() + playerStats.Strength.getValue()).ToString();

            if(statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.getValue() + playerStats.Strength.getValue()).ToString();

            if(statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.getValue() + playerStats.Agility.getValue()).ToString();

            if(statType == StatType.evasion)
                statValueText.text = (playerStats.Evasion.getValue()+playerStats.Agility.getValue()).ToString() ;

            if(statType == StatType.magicRes)
                statValueText.text=(playerStats.magicResistance.getValue()+playerStats.Intelligence.getValue()*3).ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
