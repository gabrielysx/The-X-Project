using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    private int typeID,itemPrice;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleName, priceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlotData(int itemID, int price)
    {
        typeID = itemID;
        itemPrice = price;
        LootItem item = InventoryManager.instance.GetLootItemInfo(typeID);
        iconImage.sprite = item.icon;
        titleName.text = item.title;
        priceText.text = itemPrice.ToString();
    }

    public void PurchaseItem()
    {
        int playerGoldAmount = InventoryManager.instance.goldAmount;
        if(itemPrice > playerGoldAmount)
        {
            Debug.Log("Not enough Gold!");
        }
        else
        {
            InventoryManager.instance.AddGold(-itemPrice);
            InventoryManager.instance.AddLoot(typeID, 1);
            UIManager.instance.UpdateGoldAmount();
            UIManager.instance.UpdateInventorySlots();
        }
    }

}
