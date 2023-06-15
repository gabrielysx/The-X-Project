using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private UIManager UImanager;
    [SerializeField] private GameObject door1, door2;
    [SerializeField] private List<Spawner> spawnerList;
    [SerializeField] private float spawningInterval = 2f;
    [SerializeField] private int maxSpawnNumber = 8;
    private bool isSpawnerActive = false;
    private bool spawnFinished;
    private float spawnerTimer;
    [SerializeField] private Transform spawnedMobsParent;
    private int curSpawnNumber;
    public GameObject lootHolder; 

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawnerActive && spawnerList.Count > 0)
        {
            spawnerTimer += Time.deltaTime;
            if(spawnerTimer >= spawningInterval)
            {
                spawnerTimer = 0f;
                if(curSpawnNumber < maxSpawnNumber)
                {
                    int randomIndex = Random.Range(0, spawnerList.Count);
                    spawnerList[randomIndex].SpawnEnemy();
                    curSpawnNumber++;
                }
                else
                {
                    isSpawnerActive = false;
                    spawnFinished = true;
                }
            }
        }
        if (spawnFinished)
        {
            if(spawnedMobsParent.childCount <= 0)
            {
                FinishChallengeRoom();
            }
        }

    }

    public void StartChallengeRoom()
    {
        door1.SetActive(true);
        isSpawnerActive = true;
    }

    public void FinishChallengeRoom()
    {
        door1.SetActive(false);
        door2.SetActive(false);
    }

}
