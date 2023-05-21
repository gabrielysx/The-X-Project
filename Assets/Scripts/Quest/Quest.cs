using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestItem", menuName = "My Assets/Quest", order = 1)]

public class Quest : ScriptableObject
{
    public int ID;
    public string title, description;
    public List<string> stepDescription = new List<string>();
    public List<int> rewardIDList = new List<int>();
    public List<int> rewardAmountList = new List<int>();
}
