using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour
{
    Movement Movement;

    // Floats
    float dashSpeed = 30f;
    float dashCooldown = 0;

    string dashDirection;

    // Vectors
    public Vector3 dashRight;
    public Vector3 dashLeft;

    // Bools
    bool dashing = false;

    Coroutine Timer;

    CharacterController controller;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        dashRight = new Vector3(0.2f, 0.0f, 0.0f);
        dashLeft = new Vector3(-0.2f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*------------------------------------ PLAYER DASHING ----------------------------*/
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldown <= 0)
        {
            dashing = true;
        }

        if (dashing == true)
        {
            if (Input.GetKey(KeyCode.D))
            {
                dashDirection = "Right";
                rb.AddForce(dashRight * dashSpeed, ForceMode.Impulse);
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dashDirection = "Left";
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
        if (dashDirection == "Right")
        {
            rb.AddForce(dashRight * -dashSpeed, ForceMode.Impulse);
        }
        else if (dashDirection == "Left")
        {
            rb.AddForce(dashLeft * -dashSpeed, ForceMode.Impulse);
        }
        StopCoroutine(Timer);
    }
}