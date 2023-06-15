using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject parent;

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        newEnemy.transform.parent = parent.transform;
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
