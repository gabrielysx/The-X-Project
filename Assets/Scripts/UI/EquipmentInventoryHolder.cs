using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventoryHolder : MonoBehaviour
{
    public int slotIndex;
    private EquipmentItem curEquipmentItem;
    [SerializeField]private GameObject draggedItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeHolder(int index,EquipmentItem item,Transform globalHolder,Canvas UIcanvas)
    {
        slotIndex= index;
        curEquipmentItem = item;
        draggedItem.GetComponent<DragableItem>().Initialize(item.icon, gameObject, globalHolder,UIcanvas);
    }

    public EquipmentItem GetCurrentItem()
    {
        return curEquipmentItem;
    }

    public void SelfDestroy()
    {
        ClearChilds(transform);
        Destroy(gameObject);
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
}
