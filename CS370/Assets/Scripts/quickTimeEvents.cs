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
 *  QTEGenerate = F, QTEType != 0, Timer = null
 *  
 *  
 *  Post-Condition:
 *  QTEFinished = T, QTEGenerate = F, Timer = null, QTEType = 0
 *  
 *  
 *  Output/Result:
 *  
 *  CASE 1: Passed QTE
 *  QTEFinished = T
 *  Success = T
 *  
 *  CASE 2: Failed QTE
 *  QTEFinished = T
 *  Success = F
 */

public class quickTimeEvents : MonoBehaviour{

    //Declaring Variables

    //General
    public static int QTEType;
    bool QTEGenerate;
    public static bool Success;
    Coroutine Timer;
    public static bool QTEFinished;

    //Key Combination
    int NumKeys;
    public static char[] KeyCombination;
    string Characters;
    int CurrentTestedKey;

    int RandomNum;

    //Button Mash
    int NumClicks;
    int CurrentClicks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

        //Initializing Variables
        QTEType = 1;                                    //[0]: Default, [1]: KeyCombination, ....           **CHANGE TO 0 WHEN DONE TESTING**
        QTEGenerate = false;                             //Triggers QTE generation                          
        Success = false;                                //Boolean result of QTE
        QTEFinished = false;                            //Detects if QTE is finished

        //Key Combination QTE
        NumKeys = 5;                                    //Number of keys required for KeyCombination
        KeyCombination = new char[NumKeys];             //KeyCombination Sequence
        Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";      //Possible character values for KeyCombination
        CurrentTestedKey = 0;                           //Key input index for KeyCombination

        //Button Mash QTE
        NumClicks = 10;
        CurrentClicks = 0;
    }

    // Update is called once per frame
    void Update(){

        //Generation Phase
        if(Timer == null){
            //Trigger Generation
            if (QTEGenerate == false && QTEType != 0)
            {

                QTEGenerate = true;

                //Reset Variables
                QTEFinished = false;
                Success = false;
                CurrentTestedKey = 0;
                CurrentClicks = 0;
            }

            //Do the Generation
            if(QTEGenerate == true){

                if (QTEType == 1){                                               //If the Type is Key Combination QTE

                    //Generate Key Combination
                    for (int i = 0; i < NumKeys; i++){

                        RandomNum = UnityEngine.Random.Range(0, 26);            //Random number in set [0,26)
                        KeyCombination[i] = Characters[RandomNum];              //Assigns key sequence in slot i
                    }

                    //Show Combination in Console
                    for (int i = 0; i < NumKeys; i++){

                        Debug.Log(KeyCombination[i]);
                    }

                    //Start Timer
                    Timer = StartCoroutine(TimerCoroutine(5));
                }
                else if (QTEType == 2){                                        //If the Type is Key Combination QTE

                    //Start Timer
                    Timer = StartCoroutine(TimerCoroutine(5));
                }

                //Stop Generation
                QTEGenerate = false;
            }
        }

        //Test Input Phase
        if (Timer != null){
            if (QTEType == 1){                                  //If the Type is Key Combination QTE

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

                    Success = true;                                             //Player was successful
                    QTEFinished = true;                                         //QTE is finished
                    QTEType = 0;                                                //Reset QTE type to Default

                    //Stop Timer
                    StopCoroutine(Timer);
                    Timer = null;
                }
            }
            else if (QTEType == 2){                                             //If the Type is Button Mash QTE

                //When Space is Pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CurrentClicks++;
                    Debug.Log("Space Pressed");
                }
                
                //If Required Number of Clicks is Reached
                if (CurrentClicks >= NumClicks){

                    Debug.Log("QTE Completed Successfully!");

                    Success = true;                                             //Player was successful
                    QTEFinished = true;                                         //QTE is finished
                    QTEType = 0;                                                //Reset QTE type to Default

                    //Stop Timer
                    StopCoroutine(Timer);
                    Timer = null;
                }

            }
        }
    }

    //QTE Timer
    IEnumerator TimerCoroutine(int Seconds){

        //Start Timer
        yield return new WaitForSeconds(Seconds);

        //If Not Completed By Then
        if(Timer != null && Success == false){

            Timer = null;
            QTEFinished = true;

            QTEType = 0;                     //Reset QTE Type to Default

            Debug.Log("QTE Failed");
        }     
    }
}
