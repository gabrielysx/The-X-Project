using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    //Weapon List
    [SerializeField] private List<GameObject> bulletPrefabs;
    [SerializeField] private List<GameObject> weapons;

    //Other weapon configurations
    [SerializeField] private float slashRange = 1f;
    [SerializeField] private float slashAngle = 60f;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float multiSpreadAngle;
    
    //Weapon Slot
    [SerializeField] private GameObject leftSlot,rightSlot;
    private bool isLeftSlotEmpty, isRightSlotEmpty;
    private int leftWeaponID, rightWeaponID;

    //Attack variables
    private bool isLeftFiring, isRightFIring;
    [SerializeField] private Camera cam;
    [SerializeField] private float curLeftRate = 2f;
    [SerializeField] private float curRightRate = 1f;
    private float leftTimer, rightTimer;

    // Start is called before the first frame update
    void Start()
    {
        //bulletHolder = GameObject.Find("BulletHolder");
        if(leftSlot == null)
        {
            leftSlot = transform.GetChild(0).gameObject;
        }
        if (rightSlot == null)
        {
            rightSlot = transform.GetChild(1).gameObject;
        }
        isLeftSlotEmpty = true;
        isRightSlotEmpty = true;
        LeftSlotUpdate(InventoryManager.instance.leftWeapon);
        RightSlotUpdate(InventoryManager.instance.rightWeapon);
        cam = Character.mainPlayerInstance.GetMainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        LeftAttack();
        RightAttack();
    }

    private void LeftAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isLeftFiring = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isLeftFiring = false;
        }

        if (!isLeftFiring)
        {
            if (leftTimer > 0f)
            {
                leftTimer += Time.deltaTime;
                if (leftTimer > (1 / curLeftRate))
                {
                    leftTimer = 0f;
                }
            }
        }
        else
        {

            if (leftTimer == 0f)
            {
                //Get mouse position and direction
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = pos - transform.position;

                //Shot the specific weapon in the left slot
                leftSlot.transform.GetChild(0).GetComponent<IWeaponHandler>().Fire(dir,transform.position);

                //Update Timer
                leftTimer += Time.deltaTime;
            }
            else if (leftTimer <= (1 / curLeftRate))
            {
                leftTimer += Time.deltaTime;
            }
            else
            {
                leftTimer = 0f;
            }
        }
    }

    private void RightAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRightFIring = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRightFIring = false;
        }

        if (!isRightFIring)
        {
            if (rightTimer > 0f)
            {
                rightTimer += Time.deltaTime;
                if (rightTimer > (1 / curRightRate))
                {
                    rightTimer = 0f;
                }
            }
        }
        else
        {

            if (rightTimer == 0f)
            {
                //Get mouse position and direction
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = mousePos - transform.position;

                //Shot the specific weapon in the right slot
                rightSlot.transform.GetChild(0).GetComponent<IWeaponHandler>().Fire(dir, transform.position);

                //Set up timer
                rightTimer += Time.deltaTime;
            }
            else if (rightTimer <= (1 / curRightRate))
            {
                rightTimer += Time.deltaTime;
            }
            else
            {
                rightTimer = 0f;
            }
        }

    }

    public void LeftSlotUpdate(int newID)
    {
        if (newID == -1)
        {
            isLeftSlotEmpty = true;
        }
        else
        {
            isLeftSlotEmpty = false;
            leftWeaponID = newID;
        }
        LeftSlotInitialize();
    }

    public void RightSlotUpdate(int newID)
    {
        if (newID == -1)
        {
            isRightSlotEmpty = true;
        }
        else
        {
            isRightSlotEmpty = false;
            rightWeaponID = newID;
        }
        RightSlotInitialize();
    }

    private void LeftSlotInitialize()
    {
        if(isLeftSlotEmpty)
        {
            leftWeaponID = 0;
        }
        if(leftSlot.transform.childCount > 0)
        {
            Destroy(leftSlot.transform.GetChild(0).gameObject);
        }
        GameObject newWeapon = Instantiate(weapons[leftWeaponID],leftSlot.transform);
        curLeftRate = newWeapon.GetComponent<IWeaponHandler>().GetFireRate();
    }

    private void RightSlotInitialize()
    {
        if(isRightSlotEmpty)
        {
            rightWeaponID = 1;
        }
        if(rightSlot.transform.childCount > 0)
        {
            Destroy(rightSlot.transform.GetChild(0).gameObject);
        }
        GameObject newWeapon = Instantiate(weapons[rightWeaponID], rightSlot.transform);
        curRightRate = newWeapon.GetComponent<IWeaponHandler>().GetFireRate();
    }
}


