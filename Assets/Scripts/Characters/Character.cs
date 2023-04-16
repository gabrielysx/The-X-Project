using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Basic components and variables for the character
    private CharacterStateManager CSM;
    private Rigidbody2D rb;
    private int currentHP;
    [SerializeField] private int baseHP;
    [SerializeField] private float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        CSM= GetComponent<CharacterStateManager>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = baseHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IfMoving()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveToTargetPoint()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 pos = gameObject.transform.position;
        pos += dir.normalized * baseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);

    }

}
