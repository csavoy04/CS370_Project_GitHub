using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public Animator Animator;

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
        Animator = GetComponent<Animator>();
    }

    void resetAnim()
    {
        Animator.SetBool("idle", true);
        Animator.SetBool("goback", false);
        Animator.SetBool("goleft", false);
        Animator.SetBool("goright", false);
        Animator.SetBool("gofront", false);
        Animator.SetBool("jump", false);
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
        resetAnim();

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
                rb.MovePosition(rb.position + Vector3.ClampMagnitude(Vector3.forward, 1) * Time.deltaTime * speed);
                Animator.SetBool("goback", true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.MovePosition(rb.position + Vector3.ClampMagnitude(Vector3.back, 1) * Time.deltaTime * speed);
                Animator.SetBool("gofront", true);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.MovePosition(rb.position + Vector3.ClampMagnitude(Vector3.left, 1) * Time.deltaTime * speed);
                Animator.SetBool("goleft", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.MovePosition(rb.position + Vector3.ClampMagnitude(Vector3.right, 1) * Time.deltaTime * speed);
                Animator.SetBool("goright", true);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                Jump();
                Animator.SetBool("jump", true);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CombatTrigger") || other.gameObject.CompareTag("Enemy"))
        {

            if(other.gameObject.name == "Slime")
            {
                GameHandler.Instance.CurrentCombatArea = GameHandler.CombatAreaName.SlimeField;
            }

            Debug.Log("Entering Combat Area");
            SceneManager.LoadScene("CombatArea");
        }
    } 
}