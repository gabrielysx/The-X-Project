using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private float inputCDtime = 0.6f;
    private bool inputCooldown;
    [SerializeField] private GameObject questInfoPanel;
    [SerializeField] private List<GameObject> NPCs;
    private float inputCooldownTimer;
    [SerializeField] private GameObject inventoryPanel, slotsHolder, slotsPrefab, goldAmountText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject NPC in NPCs)
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
                if(inventoryPanel.activeInHierarchy)
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

    public void UpdateInventorySlots()
    {
        foreach(Transform child in slotsHolder.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(BagLoots loot in InventoryManager.instance.loots)
        {
            
            GameObject slot = Instantiate(slotsPrefab, slotsHolder.transform);
            LootItem cur = InventoryManager.instance.GetLootItemInfo(loot.typeID);
            //update image and number
            slot.transform.GetChild(0).GetComponent<Image>().sprite = cur.icon;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = loot.amount.ToString();
        }
    }

    public void UpdateGoldAmount()
    {
        goldAmountText.GetComponent<TMP_Text>().text = InventoryManager.instance.goldAmount.ToString();
    }

}
