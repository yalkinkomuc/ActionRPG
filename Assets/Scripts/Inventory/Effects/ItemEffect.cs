using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;
   public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect Executed");
    }


}
