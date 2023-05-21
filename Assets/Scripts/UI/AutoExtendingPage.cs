using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoExtendingPage : MonoBehaviour
{
    public int amount_each_line, amount_in_first_page, slot_height;
    public int count;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        count = gameObject.transform.childCount;
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
