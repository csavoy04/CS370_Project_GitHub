using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.LowLevelPhysics;

/*  Script made by:
        Script that:
            - obtains the current direction of the player as a Vector3
            - allows the player to dash 5 units ahead, or obj detection -0.5
                in the event of the raycast colliding with an object
            - allows the player to climb upon approaching a climbable wall
    Date: 9/30/2025
    Made in: C# VsCode
*/

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour
{
    Movement Movement;

    // Floats
    float dashCooldown = 0;
    float dashDistance = 5;
    float speed;

    string dashDirection;
    float speed;

    // Vectors
    public Vector3 direction;

    // Bools
    bool dashing = false;
    bool grounded;
    bool moveable;
    bool climbing = false;

    Coroutine Timer;

    CharacterController controller;
    Rigidbody rb;
    RaycastHit hit;
    int CLIMABLE = LayerMask.GetMask("CLIMABLE");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Obtaining external values
        bool grounded = GameObject.Find("Player").GetComponent<Movement>().grounded;
        bool moveable = GameObject.Find("Player").GetComponent<Movement>().moveable;
        float speed = GameObject.Find("Player").GetComponent<Movement>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        /*------------------------------------ PLAYER DASHING ----------------------------*/
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldown <= 0 && moveable)
        {
            dashing = true;
        }

        if (dashing == true)
        {
            /* Obtains current direction and store it as direction, if there is nothing
            detected in front of the player then the player will dash in it's
            current direction 5 units. If the raycast detects an object then the 
            dash distance is change to that of collision distance - 0.5 */

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

        /*---------------------------------------- CLIMBING ----------------------------------*/
        if (Physics.Raycast(transform.position, direction, 0.5f, CLIMABLE) && !grounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Stopping normal player movement, disables gravity, resets player velocity
            moveable = false;
            rb.linearVelocity = Vector.zero;
            rb.useGravity = false;

            /*-------------------------------- CLIMBING ACTION ------------------------------- */
            if (Input.GetKeyDown(KeyCode.A) && climbing)
            {
                transform.Translate((direction*Quaternion.AngleAxis(90, Vector3.left))*Time.deltaTime*(speed/2));
            } else {rb.velocity = Vector.zero;}

            if (Input.GetKeyDown(KeyCode.D) && climbing)
            {
                transform.Translate((direction*Quaternion.AngleAxis(90, Vector3.right))*Time.deltaTime*(speed/2));
            } else {rb.velocity = Vector.zero;}

            if (Input.GetKeyDown(KeyCode.W) && climbing)
            {
                transform.Translate(Vector3.up * Time.deltaTime * (speed / 2));
            }

            if (Input.GetKeyDown(KeyCode.S) && climbing)
            {
                transform.Translate(Vector3.down * Time.deltaTime * (speed / 2));
            }
        }
        // Resets boolean statements
        else
        {
            moveable = true;
            rb.useGravity = true;
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