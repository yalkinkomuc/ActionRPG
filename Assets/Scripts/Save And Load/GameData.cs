using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{
    public int currency;

    public SerializableDictionary<string, bool> skilltree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string,bool> checkpoints;
    public string closestCheckpointID;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string, float> volumeSettings;
    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        
        this.currency = 0;

        skilltree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        closestCheckpointID = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
        volumeSettings = new SerializableDictionary<string, float>();
    }
}
