using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private Image image;
    private EquipmentInventoryHolder parentInventoryHolder;
    public EquipmentType curType;
    private Transform globalHolder, originalHolder;

    [SerializeField]private RectTransform rect;
    [SerializeField]private CanvasGroup CG;
    private Canvas canvas;
    private Vector2 originalPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        CG.blocksRaycasts = false;
        gameObject.transform.SetParent(globalHolder);
        originalPos = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CG.blocksRaycasts = true;
        rect.anchoredPosition = originalPos;
        gameObject.transform.SetParent(originalHolder);
    }

    //mouse hover on the icon will show the item's info
    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    public void Initialize(Sprite icon, GameObject parent, Transform moveHolder, Canvas UIcanvas) 
    {
        canvas = UIcanvas;
        image.sprite = icon;
        parentInventoryHolder = parent.GetComponent<EquipmentInventoryHolder>();
        curType = parentInventoryHolder.GetCurrentItem().type;
        globalHolder = moveHolder;
        originalHolder = transform.parent;
    }

    private void ClearPreviousSlot()
    {
        parentInventoryHolder.SelfDestroy();
    }

    public void DestroyWhenFinishDropping()
    {
        ClearPreviousSlot();
        //clean up this Game Object
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public int GetCurrentItemID()
    {
        return parentInventoryHolder.GetCurrentItem().ID;
    } 

    public int GetParentInventorySlotIndex()
    {
        return parentInventoryHolder.slotIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
