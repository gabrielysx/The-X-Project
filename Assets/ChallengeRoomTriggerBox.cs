using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeRoomTriggerBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.StartChallengeRoom();
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
