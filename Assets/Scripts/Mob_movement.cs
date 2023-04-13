using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Mob_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool moving;
    private float current_time;
    private Vector2 current_dir;
    [SerializeField]
    private float velocity, move_interval;
    public bool isIdle, isRandomMoving, isChasing;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        isIdle = true;
        isRandomMoving = false;
        isChasing = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            current_dir = behaviour_state();
            basic_movement(current_dir);
            moving = true;
            current_time = 0;
        }
        else
        {
            basic_movement(current_dir);
            current_time += Time.deltaTime;
        }
        if(current_time > move_interval)
        {
            moving = false;
            current_dir = new Vector2(0f, 0f);
        }

    }

    private Vector2 behaviour_state()
    {
        float x, y;
        if (isIdle)
        {
            x = 0f;
            y = 0f;
        }
        else if (isChasing)
        {
            x = 0f;
            y = 0f;
            //find player and chasing
        }
        else if (isRandomMoving)
        {
            x = (Random.value * 2) - 1;
            y = (Random.value * 2) - 1;
        }
        else
        {
            x = 0f;
            y = 0f;
        }
        return new Vector2(x, y);
    }

    private void basic_movement(Vector2 dir)
    {
        rb.velocity = dir.normalized * velocity;
    }
}
