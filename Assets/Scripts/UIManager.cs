using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver
{
    public static UIManager instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private float inputCDtime = 0.6f;
    private bool inputCooldown;
    private float inputCooldownTimer;

    //Quest Info
    [SerializeField] private GameObject questInfoPanel, questSlotPrefab;
    [SerializeField] private Transform questSlotHolder;

    //Quest Choice
    [SerializeField] private GameObject questChoicePanel;
    [SerializeField] private Transform choiceQuestSlotHolder;

    [SerializeField] private List<GameObject> NPCs;

    //inventory panel
    [SerializeField] private GameObject inventoryPanel, itemSlotsHolder, slotsPrefab, equipSlotsHolder, equipSlotsPrefab, goldAmountText;
    public List<EquipmentSlot> equipSlots;
    public WeaponSlot leftWeaponSlot, rightWeaponSlot;

    //GlobalHolder
    [SerializeField] private Transform globalHolder;

    //HP and numbers
    [SerializeField] private GameObject dmgNumberPrefab, worldSpaceCanvas;
    private void Awake()
    {

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        QuestManager.instance.AddObserver(this);
    }

    public void OnNotify()
    {
        UpdateQuestInfoPanel();
        UpdateQuestChoiceList();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject NPC in NPCs)
        {
            InteractionFlag flag = NPC.transform.GetComponent<InteractionFlag>();
            if (flag.getFlagofInteractable())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    flag.NPC_DialoguePanel.SetActive(!flag.NPC_DialoguePanel.activeInHierarchy);
                }
            }
        }

        if (!inputCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                questInfoPanel.SetActive(!questInfoPanel.gameObject.activeInHierarchy);
                inputCooldown = true;
                inputCooldownTimer = 0;
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
                if (inventoryPanel.activeInHierarchy)
                {
                    UpdateInventorySlots();
                    UpdateGoldAmount();
                }
                inputCooldown = true;
                inputCooldownTimer = 0;
            }
        }
        else
        {
            inputCooldownTimer += Time.deltaTime;
            if (inputCooldownTimer > inputCDtime)
            {
                inputCooldown = false;
            }
        }
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

    public void UpdateInventorySlots()
    {
        //refresh the item inventory slots
        ClearChilds(itemSlotsHolder.transform);
        foreach (BagLoots loot in InventoryManager.instance.loots)
        {

            GameObject slot = Instantiate(slotsPrefab, itemSlotsHolder.transform);
            LootItem cur = InventoryManager.instance.GetLootItemInfo(loot.typeID);
            //update image and number
            slot.transform.GetChild(0).GetComponent<Image>().sprite = cur.icon;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = loot.amount.ToString();
        }
        //refresh the equipment inventory slots
        ClearChilds(equipSlotsHolder.transform);
        foreach (int itemIndex in InventoryManager.instance.equipments)
        {
            int i = InventoryManager.instance.equipments.IndexOf(itemIndex);
            GameObject slot = Instantiate(equipSlotsPrefab, equipSlotsHolder.transform);
            slot.GetComponent<EquipmentInventoryHolder>().InitializeHolder(i, InventoryManager.instance.equipmentTypes[itemIndex], globalHolder, canvas);
        }

        //refresh equipment slots
        foreach (EquipmentSlot slot in equipSlots)
        {
            int i = equipSlots.IndexOf(slot);
            //refresh equipment slots
            slot.UpdateSlot(InventoryManager.instance.equipmentSlots[i]);

        }

        //refresh weapon slots
        leftWeaponSlot.UpdateSlot(InventoryManager.instance.leftWeapon);
        rightWeaponSlot.UpdateSlot(InventoryManager.instance.rightWeapon);
    }

    public void UpdateGoldAmount()
    {
        goldAmountText.GetComponent<TMP_Text>().text = InventoryManager.instance.goldAmount.ToString();
    }

    public void UpdateQuestInfoPanel()
    {
        List<Quest> qList = QuestManager.instance.questList;
        //Clear quest slots holder
        ClearChilds(questSlotHolder);
        //generate new slots
        foreach (Quest q in qList)
        {
            int i = qList.IndexOf(q);
            GameObject questSlot = Instantiate(questSlotPrefab, questSlotHolder);
            questSlot.GetComponent<QuestSlotUIController>().RefreshQuestSlotUI(i, q);
        }
    }

    public void UpdateQuestChoiceList()
    {
        List<Quest> qList = QuestManager.instance.questChoiceList;
        //Clear quest slots holder
        ClearChilds(choiceQuestSlotHolder);
        //generate new slots
        foreach (Quest q in qList)
        {
            int i = qList.IndexOf(q);
            GameObject questSlot = Instantiate(questSlotPrefab, choiceQuestSlotHolder);
            questSlot.GetComponent<QuestSlotUIController>().RefreshQuestSlotUI(i, q);
        }
    }

    public void GenerateDMGNumber(int dmg, Vector3 pos)
    {
        GameObject dmgNumber = Instantiate(dmgNumberPrefab, pos, new Quaternion(0, 0, 0, 0), worldSpaceCanvas.transform);
        dmgNumber.transform.GetChild(0).GetComponent<TMP_Text>().text = "- " + dmg.ToString();
    }
}
