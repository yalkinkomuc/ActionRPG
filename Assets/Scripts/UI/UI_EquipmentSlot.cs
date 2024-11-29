using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType SlotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot -"+SlotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item==null || item.data == null)
            return;


        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}
