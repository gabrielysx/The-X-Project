using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionFlag : MonoBehaviour
{
    private bool flagofInteractable = false;
    public GameObject NPC_DialoguePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getFlagofInteractable()
    {
        return flagofInteractable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            flagofInteractable = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            flagofInteractable = false;
        }
    }

}
