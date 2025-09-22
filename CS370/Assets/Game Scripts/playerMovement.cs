using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Script made by: Coleman
        - Script that takes in the user's input and corelates it to a given transform command
        - W and S controls fowrard speed
        - A and D controls side speed
        - Space bar controls vertical speed

    Date: 9/18/25
    Made in: C# VScode
*/
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public Vector3 moveDirection;
    public Vector3 jump;
    public Vector3 dashRight;
    public Vector3 dashLeft;

    // Booleans
    public bool isCrouching = false;
    public bool isRunning = false;
    public bool grounded = true;
    public bool dashing = false;

    // Floats
    public float speed = 8.0f;
    public float jumpHeight = 4;

    // Dashing floats
    public float dashSpeed = 2f;
    public float dashCooldown = 0.0f;
    string DashDirection;

    Coroutine Timer;

    CharacterController controller;
    Rigidbody rb;

    // Ran at the start of the script being ran
    void Start()
    {
        transform.Translate(new Vector3(0, 0, 0)); // Starting orientation
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);
        dashRight = new Vector3(0.2f, 0.0f, 0.0f);
        dashLeft = new Vector3(-0.2f, 0.0f, 0.0f);
    }

    private void OnCollisionStay()
    {
        grounded = true;
    }

    // Updates per frame
    void Update()
    {

        // Speed controler
        if (isRunning)
        {
            speed = 13.0f;
        }
        else if (isCrouching)
        {
            speed = 3.0f;
        }
        else
        {
            speed = 10.0f;
        }

        /* ----------------------------- INPUT STATEMENTS ----------------------------*/
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.AddForce(jump * jumpHeight, ForceMode.Impulse);
            grounded = false;
        }

        if (Input.GetKey(KeyCode.C) && grounded && isRunning == false)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && grounded)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }


        /*------------------------------------ PLAYER DASHING ----------------------------*/
        if (Input.GetKeyDown(KeyCode.LeftControl) && grounded == false && dashCooldown <= 0)
        {
            dashing = true;
        }

        if (dashing == true) {
            if (Input.GetKey(KeyCode.D))
            {
                DashDirection = "Right";
                rb.AddForce(dashRight * dashSpeed, ForceMode.Impulse);
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            else
            {
                DashDirection = "Left";
                rb.AddForce(dashLeft * dashSpeed, ForceMode.Impulse);
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
        }

        // Dash cooldown
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    IEnumerator TimerCoroutine(float Seconds)
    {

        //Start Timer
        yield return new WaitForSeconds(Seconds);
        dashCooldown = 2.0f;
        dashing = false;
        if(DashDirection == "Right")
            rb.AddForce(dashRight * -dashSpeed, ForceMode.Impulse);
        else if(DashDirection == "Left")
            rb.AddForce(dashLeft * -dashSpeed, ForceMode.Impulse);
        StopCoroutine(Timer);
    }
}