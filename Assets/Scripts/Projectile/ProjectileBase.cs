using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Vector2 flyDir;
    private Rigidbody2D rb;
    private float timer = 0f;
    private float MaxExsistTime = 2f;
    private float Speed = 3f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit!");
            GameObject.Destroy(transform.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //Fly towards target direction
        Vector2 pos = gameObject.transform.position;
        pos += flyDir.normalized * Speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);

        //If exceed the exsisting time, then self destroyed.
        timer += Time.deltaTime;
        if (timer >= MaxExsistTime)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }
}
