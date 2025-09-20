using UnityEngine;

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
    public bool isCrouching = false;
    public bool isRunning = false;
    public float speed = 5.0f;

    // Ran at the start of the script being ran
    void Start(){
        transform.Translate(new Vector3(0,0,0)); // Starting orientation
    }

    // Updates per frame
    void Update(){
        /*  Per update, when the player inputs the given button (Denoted by getButtonDown("buttonName")),
            the script moves the player (with respect to isRunning)
        */

        // Speed controler
        if(isRunning==true){
            speed = 8.0f;
        } else if(isCrouching==true){
            speed = 2.5f;
        } else{
            speed = 5.0f;
        }

        
        // Input statements
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime * (speed));
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        
    }
}