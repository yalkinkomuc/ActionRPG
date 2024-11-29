using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemsToDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
   

    public virtual void GenerateDrop()
    {
        
        Debug.Log("itworked");
        if(possibleDrop.Length == 0)
        {
            Debug.Log("ItemPool is Empty");
            return;
        }

        foreach(ItemData item in possibleDrop)
        {
            if(item != null && Random.Range(0,100)<item.dropChance)
                dropList.Add(item);
        }

        for(int i = 0; i < maxItemsToDrop; i++)
        {
            if (dropList.Count > 0)
            {
                int randomIndex = Random.Range(0, dropList.Count); // 0 ya da 1 olcak
                ItemData itemtoDrop = dropList[randomIndex];
                Debug.Log(randomIndex);
                DropItem(itemtoDrop);
                dropList.Remove(itemtoDrop);
            }
        }
    }



    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5,5),Random.Range(15,20));
        
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
  
}
