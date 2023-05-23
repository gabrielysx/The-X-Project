using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoExtendingPage : MonoBehaviour
{
    [SerializeField] private int amount_each_line, amount_in_first_page, slot_height;
    private int count, newCount;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        newCount = gameObject.transform.childCount;
        if (newCount != count)
        {
            count = newCount;
            if (count > amount_in_first_page)
            {
                float temp = (float)Mathf.Ceil((float)count / amount_each_line);
                gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slot_height * temp);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slot_height * (amount_in_first_page / amount_each_line));
            }
        }
        
    }
}
