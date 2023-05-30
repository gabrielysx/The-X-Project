using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneTriggerBox : MonoBehaviour
{
    public bool interactableFlag;
    [SerializeField]private int targetSceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactableFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactableFlag = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactableFlag && Input.GetKeyDown(KeyCode.E))
        {
            LevelManager.instance.LoadTargetScene(targetSceneIndex);
            interactableFlag = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
