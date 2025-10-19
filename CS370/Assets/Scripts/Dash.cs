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
    [Header("References")]
    Movement Movement;
    Coroutine Timer;
    RaycastHit hit;
    public Vector3 direction;
    public Movement pm;

    [Header("Variables")]
    float dashCooldown = 0;
    float dashDistance = 5;
    bool dashing = false;

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        /*------------------------------------ PLAYER DASHING ----------------------------*/
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldown <= 0)
        {
            dashing = true;
        }

        if (dashing == true)
        {
            /* Obtains current direction and store it as direction, if there is nothing
            detected in front of the player then the player will dash in it's
            current direction 5 units. If the raycast detects an object then the 
            dash distance is change to that of collision distance - 0.5 */

            if (Physics.Raycast(transform.position, direction, 5.25f))
            { 
                dashDistance = hit.distance - 0.25f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            else
            {
                dashDistance = 5.0f;
                Timer = StartCoroutine(TimerCoroutine(0.1f));
            }
            transform.Translate(direction * dashDistance);
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