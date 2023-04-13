using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    //private GameObject bulletHolder;
    private bool firing;
    [SerializeField] private Camera cam;
    [SerializeField] private float firingRate = 1f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //bulletHolder = GameObject.Find("BulletHolder");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            firing = false;
        }

        if (!firing)
        {
            if (timer > 0f)
            {
                timer += Time.deltaTime;
                if (timer > (1 / firingRate))
                {
                    timer = 0f;
                }
            }
        }
        else
        {

            if (timer == 0f)
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = pos - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(0, 0, angle-90f);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
                bullet.GetComponent<ProjectileBase>().flyDir = dir;
                timer += Time.deltaTime;
            }
            else if (timer <= (1 / firingRate))
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f;
            }
        }
    }
}
