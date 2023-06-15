using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotID;
    [SerializeField] private int defaultWeaponID;
    public bool isFull;
    public int equipmentID;
    public Image icon;

    private bool isInteractable;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInteractable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInteractable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        equipmentID = defaultWeaponID;
        isFull = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (isFull)
            {
                //Press right mouse button to remove the current item in slot
                if (Input.GetMouseButton(1))
                {
                    //Remove the current item in the weapon slot
                    if(slotID == 0)
                    {
                        InventoryManager.instance.UpdateLeftWeaponSlot(-1);
                    }
                    else
                    {
                        InventoryManager.instance.UpdateRightWeaponSlot(-1);
                    }
                    
                    UIManager.instance.UpdateInventorySlots();
                    isFull = false;
                }
            }
        }
    }

    public void UpdateSlot(int id)
    {
        if (id == -1)
        {
            isFull = false;
            equipmentID = defaultWeaponID;
            icon.sprite = InventoryManager.instance.equipmentTypes[equipmentID].icon;
            icon.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            isFull = true;
            equipmentID = id;
            icon.sprite = InventoryManager.instance.equipmentTypes[equipmentID].icon;
            icon.color = new Color(1, 1, 1, 1);
        }
    }
}
