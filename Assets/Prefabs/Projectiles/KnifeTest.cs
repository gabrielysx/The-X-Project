using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeTest : MonoBehaviour
{
    public List<Collider2D> colliders = new List<Collider2D>();
    public int index = 0;

    public void NextCollider()
    {
        colliders[index].enabled = false;
        index++;
        index = index % colliders.Count;
        colliders[index].enabled = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
