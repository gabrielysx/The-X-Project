using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUIController : MonoBehaviour
{
    [SerializeField] private GameObject requirementPrefab, rewardPrefab;
    [SerializeField] private TMP_Text indexNumberText, statusText;
    [SerializeField] private Transform requirementHolder, rewardHolder;
    [SerializeField] private GameObject submitButton;
    private List<GameObject> requirementEntries, rewardEntries;
    private int index;
    private QuestState currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitButtonPressed()
    {
        QuestManager.instance.SubmitQuest(index);
    }

    public void AcceptButtonPressed()
    {
        QuestManager.instance.AcceptQuest(index);
    }

    private void ClearChilds(Transform parent)
    {
        if (parent.childCount > 0)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    public void RefreshQuestSlotUI(int questIndex,Quest curQuest)
    {
        //get index and state
        index = questIndex;
        currentState = curQuest.state;
        //Set text of index and state
        if(currentState == QuestState.Inactive)
        {
            indexNumberText.text = "Quest " + (index + 1).ToString();
        }
        else
        {
            indexNumberText.text = (index + 1).ToString() + "#";
        }
        //update the button at the same time
        switch (currentState)
        {
            case QuestState.Inactive:
                statusText.text = "";
                submitButton.GetComponent<Button>().interactable = true;
                submitButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Accept";
                submitButton.GetComponent<Button>().onClick.RemoveAllListeners();
                submitButton.GetComponent<Button>().onClick.AddListener(AcceptButtonPressed);
                break;
            case QuestState.Active:
                statusText.text = "Status: " + "<color=\"yellow\">"+"incompleted";
                submitButton.GetComponent<Button>().interactable = false;
                break;
            case QuestState.Complete:
                statusText.text = "Status: " + "<color=\"green\">" + "completed";
                submitButton.GetComponent<Button>().interactable = true;
                submitButton.GetComponent<Button>().onClick.RemoveAllListeners();
                submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
                break;
        }
        //Set up Requirements and rewards
        //clear the childs
        ClearChilds(requirementHolder);
        ClearChilds(rewardHolder);
        //Set requirement entries
        foreach(QuestEntry entry in curQuest.entries)
        {
            int i = curQuest.entries.IndexOf(entry);
            GameObject newEntry = Instantiate(requirementPrefab, requirementHolder);
            //Update description
            string tempS = (i+1).ToString() + ". " + entry.description;
            newEntry.GetComponent<TMP_Text>().text = tempS;
            //Update amount text
            tempS = curQuest.currentAmount[i].ToString() + "/" + entry.amountNeeded.ToString();
            newEntry.transform.GetChild(0).GetComponent<TMP_Text>().text = tempS;
        }
        //Set reward entries
        //Set gold rewards
        if(curQuest.rewardGold > 0)
        {
            GameObject goldEntry = Instantiate(rewardPrefab, rewardHolder);
            //Update the icon
            goldEntry.transform.GetChild(0).GetComponent<Image>().sprite = InventoryManager.instance.GoldIcon;
            //Update the amount
            goldEntry.transform.GetChild(2).GetComponent<TMP_Text>().text = curQuest.rewardGold.ToString(); ;
        }
        //Set other rewards
    }

}
