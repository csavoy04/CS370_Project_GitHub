using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour
{
    Movement Movement;

    // Floats
    float dashCooldown = 0;
    float dashDistance = 5;

    string dashDirection;

    // Vectors
    public Vector3 direction;

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
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (Physics.Raycast(transform.position, direction, 5.5f))
            {
                dashDistance = 5.0f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            else
            {
                dashDistance = hit.distance - 0.5f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            transform.position += direction * dashDistance;
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
        StopCoroutine(Timer);
    }
}

