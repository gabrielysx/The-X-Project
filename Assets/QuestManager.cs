using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum QuestState
{
    Inactive,
    Active,
    Complete
}
[Serializable]
public enum QuestActionType
{
    Kill,
    Collect,
    Talk
}
[Serializable]
public struct EnemyType
{
    public int ID;
    public String Name;
}

public struct QuestData
{
    public QuestActionType actionType;
    public int QuestTargetID;
    public int amountChange;
    public bool rollOver; //will the value roll over to the next quest
    public bool isConsumed; //is the data consumed after use
}

[Serializable]
public struct QuestEntry
{
    public QuestActionType actionType;
    public int QuestTargetID;
    public int amountNeeded;
    public string description;
}

[Serializable]
public class Quest
{
    public QuestState state;
    public List<QuestEntry> entries;
    public List<int> currentAmount;
    public List<bool> isCompleted;

    public int UpdateAmount(int amount,int ID)
    {
        currentAmount[ID] += amount;
        if (currentAmount[ID] >= entries[ID].amountNeeded)
        {
            isCompleted[ID] = true;
            //Check if the whole quest is completed
            if(CheckIfAllEntriesCompleted()) state = QuestState.Complete;
            return currentAmount[ID] - entries[ID].amountNeeded;
        }
        return 0;
    }

    public bool CheckIfAllEntriesCompleted()
    {
        foreach (bool itemComplete in isCompleted) 
        {
            if(!itemComplete) 
            {
                //if there is any quest entry incomplete then return false
                return false;
            }
        }
        //if all complete, then return true;
        return true;
    }
}

public class QuestManager : MonoBehaviour,ISubject
{
    public static QuestManager instance;
    public List<IObserver> observers = new List<IObserver>();
    public List<Quest> questList = new List<Quest>();
    public List<Quest> completedQuestList = new List<Quest>();
    [SerializeField] private List<QuestRule> questRules = new List<QuestRule>();
    [SerializeField] private List<EnemyType> enemyTypesList = new List<EnemyType>();
    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        questList.Add(GenerateNewQuest(questRules[0]));
    }

    private void UpdateCompletedQuests()
    {
        foreach (Quest q in questList)
        {
            if(q.state == QuestState.Complete)
            {
                completedQuestList.Add(q);
                questList.Remove(q);
            }
        }
        NotifyObservers();
    }

    public void UpdateAllQuests(QuestData questData)
    {
        int temp = questData.amountChange;
        foreach(Quest q in questList)
        {
            foreach(QuestEntry entry in q.entries) 
            { 
                int index = q.entries.IndexOf(entry);
                if(entry.actionType == questData.actionType && entry.QuestTargetID == questData.QuestTargetID)
                {
                    temp = q.UpdateAmount(questData.amountChange, index);
                    if(questData.isConsumed)
                    {
                        questData.amountChange = temp;
                    }
                }
            }
        }
        UpdateCompletedQuests();
    }

    public Quest GenerateNewQuest(QuestRule rule)
    {
        //set up the item pool and enemy pool
        List<int> curEnemyPool = new List<int>();
        List<int> curItemPool = new List<int>();
        rule.requireEnemyPool.ForEach(item => curEnemyPool.Add(item));
        rule.requireItemPool.ForEach(item => curItemPool.Add(item));
        //initialize the output quest first
        Quest output = new Quest();
        output.state = QuestState.Inactive;
        output.entries = new List<QuestEntry>();
        output.currentAmount = new List<int>();
        output.isCompleted = new List<bool>();
        //according to the entry rules, generate each entry
        foreach (QuestRuleEntry entryRule in rule.questEntryRules)
        {
            QuestEntry newEntry = new QuestEntry();
            newEntry.actionType = entryRule.actionType;
            //generate detail 
            //get a random number for amount needed
            newEntry.amountNeeded = UnityEngine.Random.Range(entryRule.minRequirementAmount, entryRule.maxRequirementAmount);
            string tempString = newEntry.amountNeeded.ToString();
            //get a random item/enemy needed from the pool
            if(newEntry.actionType == QuestActionType.Kill && curEnemyPool.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, curEnemyPool.Count);
                newEntry.QuestTargetID = curEnemyPool[randomIndex];
                curEnemyPool.RemoveAt(randomIndex);
                tempString = "Kill " + tempString + " " + enemyTypesList[newEntry.QuestTargetID].Name;
            }
            else if(newEntry.actionType == QuestActionType.Collect && curItemPool.Count > 0)
            {
                int randomi = UnityEngine.Random.Range(0, curItemPool.Count);
                newEntry.QuestTargetID = curItemPool[randomi];
                curItemPool.RemoveAt(randomi);
                tempString = "Collect " + tempString + " " + InventoryManager.instance.itemTypes[newEntry.QuestTargetID].title;
            }
            //set up description
            newEntry.description = tempString;

            //Add this entry to the quest
            output.entries.Add(newEntry);
            output.currentAmount.Add(0);
            output.isCompleted.Add(false);
        }

        return output;
    }

}
