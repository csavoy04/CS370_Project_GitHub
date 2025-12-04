using UnityEngine;
using System.Collections;
// using System.Numerics;

/*  Script made by: Coleman with help from Jaden
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
    Coroutine Timer;
    // RaycastHit hit;
    public Vector3 direction;
    public Vector3 dashDistance;
    public float distance;
    Rigidbody rb;

    [Header("Variables")]
    float dashCooldown = 0f;
    bool dashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Function that gets the player direction and sets the dash distance accordingly
    void dash()
    {
        // Old dash script scrapped due to there be so many bugs with this stupid script
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.5f, Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction * 4f, ForceMode.Impulse);

        /* if (Physics.Raycast(transform.position, direction * 10f))
        {
            distance = hit.distance - 1f;
        }
        else
        {
            distance = 10f;
        }
        if (direction.x != 0)
        {
            dashDistance = (direction.x > 0) ? new Vector3((-0.5f*distance), 0f, 0f) : new Vector3((0.5f*distance), 0f, 0f);
        }
        else if (direction.z != 0)
        {
            dashDistance = (direction.z > 0) ? new Vector3(0f, 0f, (-0.5f*distance)) : new Vector3(0f, 0f, (0.5f*distance));
        }
        transform.position += dashDistance; */
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
            dash();
            Timer = StartCoroutine(TimerCoroutine(0.1f));
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