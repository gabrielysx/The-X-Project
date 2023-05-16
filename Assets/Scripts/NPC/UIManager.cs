using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel, NPC, questPanel;
    private bool inputCooldown;
    private float inputCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NPC.transform.GetComponent<InteractionFlag>().getFlagofInteractable())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialoguePanel.SetActive(!dialoguePanel.gameObject.activeInHierarchy);
            }
        }
        if (!inputCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                questPanel.SetActive(!questPanel.gameObject.activeInHierarchy);
                inputCooldown = true;
                inputCooldownTimer = 0;
            }
        }
        else
        {
            inputCooldownTimer += Time.deltaTime;
            if (inputCooldownTimer > 1f)
            {
                inputCooldown = false;
            }
        }
    }
}
