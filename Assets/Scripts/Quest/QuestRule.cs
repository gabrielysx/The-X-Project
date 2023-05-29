using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestRuleItem", menuName = "My Assets/QuestRule", order = 1)]

public class QuestRule : ScriptableObject
{
    public List<int> requireItemPool = new List<int>();
    public List<int> requireEnemyPool = new List<int>();
    public float maxRewardGoldMultiplier, minRewardGoldMultiplier;
    public List<QuestRuleEntry> questEntryRules;
}

[Serializable]
public struct QuestRuleEntry
{
    public QuestActionType actionType;
    public int maxRequirementAmount;
    public int minRequirementAmount;
}


