using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Craft_Slot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();



    }

    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if(_data==null)
            return;

        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }


   

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.setupCraftWindow(item.data as ItemDataEquipment);
    }
}
