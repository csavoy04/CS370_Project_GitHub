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

    // Booleans
    public bool isCrouching = false;
    public bool isRunning = false;
    public bool grounded = true;
    public bool dashing = false;
    public bool moveable = true;

    // Floats
    public float speed;
    public float jumpHeight;

    CharacterController controller;
    Rigidbody rb;

    // Ran at the start of the script being ran
    void Start()
    {
        transform.Translate(new Vector3(0, 0, 0)); // Starting orientation
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    /*--------------------------- RAYCAST DETECTION FOR FLOOR --------------------------*/

    // Using FixedUpdate() as to not mess stuff up
    void FixedUpdate()
    {
        // Vector alias for down direction
        Vector3 down = transform.TransformDirection(Vector3.down);

        // Raycast that looks down 0.2 units, if interupt, grounded = true
        if (Physics.Raycast(transform.position, down, 0.2f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    /*--------------------------------- MOVEMENT CONTROLS -----------------------------*/

    // Updates per frame
    void Update()
    {
        /*---------------------------------- SPEED CONTROLER ------------------------------*/
        if (isRunning)
        {
            speed = 16.0f;
        }
        else if (isCrouching)
        {
            speed = 4.0f;
        }
        else
        {
            speed = 10.0f;
        }

        /*---------------------------------- INPUT STATEMENTS ----------------------------*/
        if (moveable)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.ClampMagnitude(Vector3.forward, 1) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.ClampMagnitude(Vector3.back, 1) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.ClampMagnitude(Vector3.left, 1) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.ClampMagnitude(Vector3.right, 1) * Time.deltaTime * speed);
            }

            /*------------------------------------- OTHER MOVEMENT KEYS ------------------------*/
            if (Input.GetKey(KeyCode.Space) && grounded)
            {
                rb.AddForce(jump * jumpHeight, ForceMode.Impulse);
            }

            /*-------------------------------------- RUNNING -----------------------------------*/
            if (Input.GetKey(KeyCode.LeftShift) && grounded)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }
    }
    /*---------------------------------- Player Trigger ----------------------------*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CombatTrigger"))
        {
            Debug.Log("Entering Combat Area");
            moveable = false;
        }
    }
}