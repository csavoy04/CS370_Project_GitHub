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
    public bool isRunning = true;
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
        if(isRunning==false){
            speed = 5.0f;
        } else if(isCrouching==true){
            speed = 2.5f;
        } else{
            speed = 8.0f;
        }

        
        // Input statements
        if(Input.GetButtonDown("w")){
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("s")){
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("a")){
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("d")){
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("space")){
            transform.Translate(Vector3.up * Time.deltaTime * (speed/2));
        }
        while(Input.GetButtonDown("left ctrl")){
            isCrouching = true;
        }
        while(Input.GetButtonDown("left shift")){
            isRunning = true;
        }
        
    }
}