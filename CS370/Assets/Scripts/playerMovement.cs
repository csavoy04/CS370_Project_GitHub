using UnityEngine;
using UnityEngine.SceneManagement;
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
    [Header("References")]
    public Vector3 moveDirection;
    public Vector3 jump;
    CharacterController controller;
    CapsuleCollider capsuleCollider;
    Rigidbody rb;

    [Header("Variables")]
    public bool isCrouching = false;
    public bool isRunning = false;
    public bool dashing = false;
    public bool moveable = true;
    public bool grounded = true;
    public float speed;

    // Function for jumping
    void Jump()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jump, ForceMode.Impulse);
    }

    // Ran at the start of the script being ran
    void Start()
    {
        transform.Translate(new Vector3(0, 0, 0)); // Starting orientation
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 15.0f, 0.0f);
        controller = GetComponent<CharacterController>();
        controller.height = 2.0f;
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.height = 2.0f;
    }

    /*--------------------------------------- RAYCAST ---------------------------------*/
    private void FixedUpdate()
    {
        // This is used for gravity
        GetComponent<Rigidbody>().AddForce(Physics.gravity*1.5f, ForceMode.Acceleration);

        // Raycast
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1f))
        {
            grounded = true;
            rb.linearVelocity = Vector3.zero;
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
        if (isRunning && moveable)
        {
            speed = 16.0f;
        }
        else if (isCrouching && moveable)
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

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                Jump();
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

            if (Input.GetKey(KeyCode.C) && grounded && !isRunning)
            {
                isCrouching = true;
                controller.height = 0.8f;
                capsuleCollider.height = 0.8f;
            }
            else
            {
                isCrouching = false;
                controller.height = 2.0f;
                capsuleCollider.height = 2.0f;
            }
        }
    }
    /*---------------------------------- Player Trigger ----------------------------*/
    /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CombatTrigger") || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Entering Combat Area");
            SceneManager.LoadScene("CombatArea");
        }
        if (other.gameObject.CompareTag("w2Trigger")){
            SceneManager.LoadScene("World2");
        }
    } */
}