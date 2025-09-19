using UnityEngine;
using System.Collections;

/*  Script written by Jaden Cheung
 *  ----------------------------------------------------------------------
 *  
 *  Purpose:
 *  This is a script that handles QTEs (Quick Time Events)
 *  
 *  
 *  Pre-Condition:
 *  Generate = T, Timer = F, Success = F
 *  
 *  
 *  Post-Condition:
 *  QTEFinished = T, Generate = F, Timer = F
 *  
 *  
 *  Output/Result:
 *  When QTEFinished = T
 *  if Success = T QTE Passed
 *  else if Success = F QTE Failed
 */

public class quickTimeEvents : MonoBehaviour{

    //Declaring Variables
    int QuickTimeEventType;
    bool Generate;
    bool Success;
    bool Timer;     
    int NumKeys;
    bool QTEFinished;

    char[] KeyCombination;
    string Characters;
    int CurrentTestedKey;

    int RandomNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

        //Initializing Variables
        QuickTimeEventType = 1;                         //[0]: Default, [1]: KeyCombination, ....
        Generate = true;                                //Triggers QTE generation
        Success = false;                                //Boolean result of QTE
        Timer = false;                                  //Triggers QTE timer
        NumKeys = 5;                                    //Number of keys required for KeyCombination
        QTEFinished = false;

        KeyCombination = new char[NumKeys];             //KeyCombination Sequence
        Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";      //Possible character values for KeyCombination
        CurrentTestedKey = 0;                           //Key input index for KeyCombination
    }

    // Update is called once per frame
    void Update(){

        //Start Generation
        if (Generate == true && Timer == false && Success == false){

            QTEFinished = false;
            Success = false;

            //If the Type is Key Combination QTE
            if (QuickTimeEventType == 1){

                //Generate Key Combination
                for (int i = 0; i < NumKeys; i++){

                    RandomNum = UnityEngine.Random.Range(0, 26);            //Random number in set [0,26)
                    KeyCombination[i] = Characters[RandomNum];              //Assigns key sequence in slot i
                }

                //Show Combination in Console
                for(int i = 0; i < NumKeys; i++){

                    Debug.Log(KeyCombination[i]);
                }
                StartCoroutine(TimerCoroutine(5));
            }
            
            //Stop Generation
            Generate = false;
        }

        //Test Input
        if (QuickTimeEventType == 1 && Timer == true){

            //If Anything is pressed
            if (Input.anyKeyDown){

                //Loop Each Key, Testing for Match
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))){

                    //If Key Pressed is the Same as Current Key Combination, Increment Tested Letter in Sequence
                    if (Input.GetKeyDown(key) && KeyCombination[CurrentTestedKey] == key.ToString()[0]){

                        CurrentTestedKey++; 
                    }
                }
            }

            //If the Whole Sequence is Complete
            if (CurrentTestedKey >= NumKeys){

                Debug.Log("QTE Completed Successfully!");

                Success = true;
                Timer = false;
                Generate = false;
                QTEFinished = true;
            }
        }
    }

    //QTE Timer
    IEnumerator TimerCoroutine(int Seconds){

        Timer = true;

        //If QTE Type is the Key Combination
        if (QuickTimeEventType == 1){

            //Start Timer
            yield return new WaitForSeconds(Seconds);
        }

        //If Not Completed By Then
        if (Timer == true && Success == false){

            Timer = false;
            Generate = false;
            QTEFinished = true;

            Debug.Log("QTE Failed");
        }     
    }
}
