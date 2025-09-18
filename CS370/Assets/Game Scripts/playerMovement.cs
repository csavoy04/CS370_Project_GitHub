using UnityEngine;
using deltaTime;
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
    public bool isCrouching = False;
    public bool isRunning = True;

    // Ran at the start of the script being ran
    void start(){
        transform.Translate = new Vector3(0,0,0); // Starting orientation
    }

    // Updates per frame
    void update(){
        /*  Per update, when the player inputs the given button (Denoted by getButtonDown("buttonName")),
            the script moves the player (with respect to isRunning)
        */

        // Speed controler
        if(isRunning==False){
            speed = 5.0;
        } elif(isCrouching==True){
            speed = 2.5;
        } else{
            speed = 8.0;
        }

        // Input statements
        if(Input.GetButtonDown("W")){
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("S")){
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("A")){
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("D")){
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if(Input.GetButtonDown("spaceBar")){
            transform.Translate(Vector3.up * Time.deltaTime * (speed/2));
        }
        while(Input.GetButtonDown("Ctrl")){
            isCrouching = True;
        }
        while(Input.GetButtonDown("Shift")){
            isRunning = True;
        }
    }
}