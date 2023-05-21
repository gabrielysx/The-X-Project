using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootItem", menuName = "My Assets/LootItem", order = 1)]
public class LootItem : ScriptableObject
{
    public int ID;
    public Sprite icon;
    public string title, description;
    public int baseValue;
}
