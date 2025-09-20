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
    public static int QTEType;
    bool QTEGenerate;
    public static bool Success;
    Coroutine Timer;
    public static bool QTEFinished;

    int NumKeys;
    char[] KeyCombination;
    string Characters;
    int CurrentTestedKey;

    int RandomNum;

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
        //To be added later
    }

    // Update is called once per frame
    void Update(){

        //Generation Phase
        if(Timer == null){
            //Trigger Generation
            if(QTEGenerate == false && QTEType != 0){

                QTEGenerate = true;
            }

            //Do the Generation
            if(QTEGenerate == true){

                QTEFinished = false;
                Success = false;

                if(QTEType == 1){                                               //If the Type is Key Combination QTE

                    //Generate Key Combination
                    for (int i = 0; i < NumKeys; i++){

                        RandomNum = UnityEngine.Random.Range(0, 26);            //Random number in set [0,26)
                        KeyCombination[i] = Characters[RandomNum];              //Assigns key sequence in slot i
                        CurrentTestedKey = 0;
                    }

                    //Show Combination in Console
                    for(int i = 0; i < NumKeys; i++){

                        Debug.Log(KeyCombination[i]);
                    }
                    Timer = StartCoroutine(TimerCoroutine(5));
                } else if(QTEType == 2){                                        //If the Type is Key Combination QTE
                    //To be added later
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
                    if(Timer != null)
    {
                        StopCoroutine(Timer);                                   //Stop Timer Coroutine
                        Timer = null;
                    }
                }
            }
            else if (QTEType == 2){                                             //If the Type is Button Mash QTE
                //To be added later
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
