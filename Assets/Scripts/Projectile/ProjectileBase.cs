using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour, IProjectile
{
    public Vector2 flyDir;
    private Rigidbody2D rb;
    private float timer = 0f;
    [SerializeField] private float MaxExsistTime = 2f;
    [SerializeField] private float Speed = 3f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDetection(collision);
        WallDetection(collision);
    }

    protected void EnemyDetection(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        Debug.Log("Hit!");
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            enemy.TakeHit(flyDir, 5, 0.3f, 0.1f);
        }
        Destroy(transform.gameObject);
        transform.GetComponent<Collider2D>().enabled = false;

    }

    protected void WallDetection(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle")) return;
        transform.GetComponent<Collider2D>().enabled = false;
        Destroy(transform.gameObject);
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

    public void SetFlyDirection(Vector2 dir)
    {
        flyDir = dir;
    }
}
