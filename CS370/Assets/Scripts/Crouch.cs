using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Script made by: Coleman
        - Script that controls player crouching

    Date: 9/23/25
    Made in: C# VScode
*/

[RequireComponent(typeof(Rigidbody))]
public class Crouch : MonoBehaviour
{
    Movement Movement;

    // Bools
    bool isCrouching;
    bool grounded;
    bool isRunning;
    bool moveable;

    CharacterController controller;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        // Obtaining external values
        bool isCrouching = GameObject.Find("Player").GetComponent<Movement>().isCrouching;
        bool grounded = GameObject.Find("Player").GetComponent<Movement>().grounded;
        bool isRunning = GameObject.Find("Player").GetComponent<Movement>().isRunning;
        bool moveable = GameObject.Find("Player").GetComponent<Dash>().moveable;
    }

    // Update is called once per frame
    void Update()
    {
        /*------------------------------------- CROUCHING ---------------------------------*/
        
        if (Input.GetKey(KeyCode.C) && grounded && !isRunning && moveable)
        {
            isCrouching = true;
            controller.height = 0.3f;
        }
        else
        {
            isCrouching = false;
            controller.height = 1.0f;
        }
    }
}
