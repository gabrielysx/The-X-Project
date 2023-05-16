using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Basic components and variables for the character
    private CharacterStateManager CSM;
    private Rigidbody2D rb;
    private int currentHP;
    private bool isDashing;
    private float dashTimer, dashCooldownTimer;
    private Vector2 dashDirection;
    [SerializeField] private Camera cam;
    [SerializeField] private bool ifDashToMousePos;
    [SerializeField] private float dashSpeed = 10, dashTime = 0.1f, dashCooldownTime = 1f;
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
        if(dashCooldownTimer <= dashCooldownTime)
        {
            dashCooldownTimer += Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                CSM.ChangeState(StateType.Dash);
                dashCooldownTimer = 0;
            }
        }
        
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

    public bool IfDashing()
    {
        return isDashing;
    }

    public void EnterDash()
    {
        isDashing = true;
        dashTimer = 0;
        if(ifDashToMousePos)
        {
            dashDirection = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }
        else
        {
            dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    public void Dash()
    {
        dashTimer += Time.fixedDeltaTime;
        if (dashTimer < dashTime)
        {
            rb.MovePosition((Vector2)transform.position + dashSpeed * Time.fixedDeltaTime * dashDirection.normalized);
        }
        else
        {
            isDashing = false;
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
