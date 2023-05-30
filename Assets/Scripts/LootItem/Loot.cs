using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    private LootItem itemInfo;
    [SerializeField] private int amount;
    [SerializeField] private float collectDistance, detectDistance;
    [SerializeField] private float initSpeed, collectSpeed, maxDuration, minDuration, detectStartTime;
    private float flyDur, timer, curSpeed;
    private Vector2 flyDirection;
    private bool isMoving, isDetecting, isCollecting;
    private Transform player;
    

    public void SetInitCondition(LootItem item, int n)
    {
        flyDirection = Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward) * Vector2.up;
        flyDur = Random.Range(minDuration, maxDuration);
        curSpeed = initSpeed;
        timer = 0;
        isMoving = true;
        isDetecting = false;
        isCollecting = false;
        itemInfo = item;
        sr.sprite = itemInfo.icon;
        amount = n;
        player = Character.mainPlayerInstance.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collectDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMoving)
        {
            Flying();
        }
        if(isDetecting)
        {
            if (Vector2.Distance(player.position, transform.position) < detectDistance)
            {
                //when detect the player
                isMoving = false;
                isDetecting = false;
                isCollecting = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
        if(isCollecting)
        {
            Vector2 dir = player.position - transform.position;
            rb.MovePosition((Vector2)transform.position + dir.normalized * collectSpeed * Time.fixedDeltaTime);
            //when reach the player, being collected to the inventory
            if(Vector2.Distance(player.position, transform.position) < collectDistance)
            {
                isCollecting = false;
                isMoving = false;
                isDetecting = false;
                InventoryManager.instance.AddLoot(itemInfo.ID, amount);
                //Send out quest data to process
                //Generate quest data
                QuestData questData = new QuestData();
                questData.actionType = QuestActionType.Collect;
                questData.QuestTargetID = itemInfo.ID;
                questData.amountChange = amount;
                questData.isConsumed = true;
                //Send out quest data to quest manager
                QuestManager.instance.UpdateAllQuests(questData);
                DestroySelf();
            }
        }
        else
        {
            if (timer > detectStartTime)
            {
                //start to detect
                isDetecting = true;
            }
            if (timer > flyDur)
            {
                //stop moving
                isMoving = false;
            }
            timer += Time.fixedDeltaTime;
        }
        
        

    }

    public void Flying()
    {
        rb.MovePosition((Vector2)transform.position + flyDirection.normalized * curSpeed * Time.fixedDeltaTime);
        curSpeed = Mathf.Lerp(curSpeed, 0, 1.5f * Time.fixedDeltaTime);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
