using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]

public class Inventory : MonoBehaviour , ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> equipment;
    public Dictionary <ItemDataEquipment, InventoryItem> equipmentDictionary;


    public List<InventoryItem> stash;
    public Dictionary <ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;


    private UI_ItemSlot[] InventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_Stat_Slot[] statSlot;

    [Header("Items Cooldown")]

    private float lastTimeUsedArmor;
    private float lastTimeUsedFlask;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        InventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_Stat_Slot>();

        addStartingItems();

    }

    private void addStartingItems()
    {

        foreach (ItemDataEquipment item in loadedEquipment)
        {
            EquipItem(item);
        }



        if(loadedItems.Count >= 0)
        {

            foreach(InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            
        }





        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;


        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;

        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifier();
        RemoveItem(_item);

        UpdateSlotUI();
    }
    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifier();
        }
    }
    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].SlotType)
                    equipmentSlot[i].UpdateSlot(item.Value);

            }
        }


        for (int i = 0; i < InventoryItemSlot.Length; i++)
        {
            InventoryItemSlot[i].CleanUpSlot();

        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();

    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem (ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment&&CanAddItem())
        
            AddToInventory(_item);
        

        else if (_item.itemType == ItemType.Material)
        
            AddToStash(_item);
        


        UpdateSlotUI();
    }
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem (ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.removeStack();    
            }
        }
        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {

            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.removeStack();
        }

        UpdateSlotUI();
    }
    public bool CanAddItem()
    {
        if(inventory.Count >= InventoryItemSlot.Length) 
        {
            
                return false;
         
        }
        return true;
    }
    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        foreach (var requireditem in _requiredMaterials)
        {
            if(stashDictionary.TryGetValue(requireditem.data,out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requireditem.stackSize)
                {
                    Debug.Log("Not Enough Materials: " + requireditem.data.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("Materials Not Found In Stash: " + requireditem.data.name);
                return false;
            }
                
            
        }

        foreach(var requiredmaterials in _requiredMaterials)
        {
            for (int i = 0; i < requiredmaterials.stackSize; i++)
            {
                RemoveItem(requiredmaterials.data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("craft is succesfull;"+_itemToCraft.name);

        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;

        }
        return equipedItem;
    }
     public void UseFlask()
    {
       ItemDataEquipment currenFlask = GetEquipment(EquipmentType.Flask);

        if ((currenFlask == null))
            return;
            
        

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currenFlask.ItemCooldown;
            currenFlask.Effect(null);
            lastTimeUsedFlask = Time.time;                            
        }
        else        
            Debug.Log("Flask on Cooldown");
        
    }

    public bool canUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor); 
        if(Time.time > lastTimeUsedArmor + armorCooldown) 
        {
            armorCooldown = currentArmor.ItemCooldown;  
            lastTimeUsedArmor = Time.time;
            return true;

        }

        Debug.Log("Armor On Cooldown");
            return false;   
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string,int> pair in _data.inventory)
        {
          foreach (var item in itemDatabase)
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach(var item in itemDatabase)
            {
                if(item != null && loadedItemId == item.itemId) 
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }

        
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();

        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }
        
        foreach (KeyValuePair<ItemData,InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment,InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void fillUpDatabase()=> itemDatabase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Items"});

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }

        return itemDatabase;
    }


#endif

}
