using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel,NPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(NPC.transform.GetComponent<InteractionFlag>().getFlagofInteractable())
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                dialoguePanel.SetActive(!dialoguePanel.gameObject.activeInHierarchy);
            }
        }
    }
}
