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
    float dashDistance = 10f;

    string dashDirection;

    // Vectors
    public Vector3 dashRight;
    public Vector3 dashLeft;

    // Bools
    bool dashing = false;

    Coroutine Timer;

    CharacterController controller;
    Rigidbody rb;
    RaycastHit hit;

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
            /* Obtains current direction and store it as fwd, if there is nothing
            detected in front of the player then the player will dash in it's
            current direction 30 units. If the raycast detects an object then the 
            dash distance is change to that of collision distance - 0.5.
            */
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, 30.5f))
            {
                dashDistance = 30.0f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            else
            {
                dashDistance = hit.distance - 0.5f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            transform.position += Vector3.forward.normalized * dashDistance;
        }

        // Dash cooldown
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

/*----------------------------------------- TIMER ------------------------------------*/
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

