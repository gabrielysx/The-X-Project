using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "My Assets/EquipmentItem", order = 2)]
public class EquipmentItem : ScriptableObject
{
    public int ID;
    public Sprite icon;
    public string title, description;
    public EquipmentType type;
    public int baseValue;
    public bool isPurchasable;
    public List<EquipmentStat> statList;
}

public enum PlayerStats
{
    HP,ATK,Speed
}

[Serializable]
public class EquipmentStat 
{
    public PlayerStats stat;
    public int value;
}
