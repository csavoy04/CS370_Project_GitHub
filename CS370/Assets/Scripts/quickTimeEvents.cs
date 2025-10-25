using UnityEngine;
using System.Collections;

/*  Script written by Jaden Cheung
    ----------------------------------------------------------------------

    Purpose:
    This is a script that handles QTEs (Quick Time Events)


    Pre-Condition:
    State == QTEState.Start, QTEType = [0,1,2]

 
    Post-Condition:
    State == QTEState.Success || State == QTEState.Fail


    Output/Result:
  
    CASE 1: Passed QTE
    State == QTEState.Success

    CASE 2: Failed QTE
    State == QTEState.Fail
*/

[System.Serializable]

public class QuickTimeEvents : MonoBehaviour
{

    //Declaring Variables

    //General
    public int QTEType;
    Coroutine Timer;
    public enum QTEState { Start, Generate, CheckUser, Success, Fail }
    public QTEState State;

    //Key Combination
    int NumKeys;
    public static char[] KeyCombination;
    string Characters;
    int CurrentTestedKey;
    public int KeyCombinationMaxDuration;

    int RandomNum;

    //Button Mash
    public static int NumClicks;
    public static int CurrentClicks;
    public int ButtonMashMaxDuration;

    //DBD Timing
    float RandomTiming;
    double CurrentTiming;
    bool DBDTimerStarted;
    public int DBDMaxDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Initializing Variables
        QTEType = 0;                                    //[0]: KeyCombination, [1]: Button Mash, [2] DBD Timing, ....
        State = QTEState.Fail;

        //Key Combination QTE
        NumKeys = 5;                                    //Number of keys required for KeyCombination
        KeyCombination = new char[NumKeys];             //KeyCombination Sequence
        Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";      //Possible character values for KeyCombination
        CurrentTestedKey = 0;                           //Key input index for KeyCombination
        KeyCombinationMaxDuration = 5;                  //Duration of KeyCombination QTE

        //Button Mash QTE
        NumClicks = 10;                                 //Number of clicks required for Button Mash QTE to finish
        CurrentClicks = 0;                              //Tracks the number of times space is pressed
        ButtonMashMaxDuration = 5;                      //Duration of Button Mash QTE

        //DBD Timing
        CurrentTiming = 0;                              //Tracks the timing of when space is pressed
        DBDTimerStarted = false;                        //Tracks if the DBD Timer has started   
        DBDMaxDuration = 2;                             //Duration of DBD Timing QTE
    }

    // Update is called once per frame
    void Update()
    {

        //Generation Phase
        if (Timer == null)
        {
            //Trigger Generation
            if (State == QTEState.Start)
            {

                State = QTEState.Generate;        //Change State to Generation Phase

                DBDTimerStarted = false;
                CurrentTestedKey = 0;
                CurrentClicks = 0;
                CurrentTiming = 0;
            }

            //Do the Generation
            if (State == QTEState.Generate)
            {

                if (QTEType == 0)
                {                                               //If the Type is Key Combination QTE

                    //Generate Key Combination
                    for (int i = 0; i < NumKeys; i++)
                    {

                        RandomNum = UnityEngine.Random.Range(0, 26);            //Random number in set [0,26)
                        KeyCombination[i] = Characters[RandomNum];              //Assigns key sequence in slot i
                    }

                    //Show Combination in Console
                    for (int i = 0; i < NumKeys; i++)
                    {

                        Debug.Log(KeyCombination[i]);
                    }

                    //Start Timer
                    Timer = StartCoroutine(TimerCoroutine(KeyCombinationMaxDuration));
                }
                else if (QTEType == 1)
                {                                        //If the Type is Button Mash QTE

                    Debug.Log("Mash Space " + NumClicks + " times!");          //Instructs player to mash space

                    //Start Timer
                    Timer = StartCoroutine(TimerCoroutine(ButtonMashMaxDuration));
                }
                else if (QTEType == 2)
                {                                         //If the Type is DBD QTE

                    RandomTiming = UnityEngine.Random.Range(54.0f, 252.0f);                 //Random number in set [54,252]
                    RandomTiming = (RandomTiming / 360) * DBDMaxDuration;                   //Calculates the actual time

                    Debug.Log("Timing window starts: " + RandomTiming);                     //Prints out the start of the timing window
                    Debug.Log("Timing window ends: " + (RandomTiming + 0.5f));              //Prints out the end of the timing window

                    //Start Timer
                    Timer = StartCoroutine(TimerCoroutine(DBDMaxDuration));
                }

                //Stop Generation
                State = QTEState.CheckUser;                                    //Change State to Input Phase
            }
        }

        //Test Input Phase
        if (State == QTEState.CheckUser && Timer != null)
        {
            if (QTEType == 0)
            {                                  //If the Type is Key Combination QTE

                //If Anything is pressed
                if (Input.anyKeyDown)
                {

                    //Loop Each Key, Testing for Match
                    foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                    {
                        //If Key is A-Z
                        if (key >= KeyCode.A && key <= KeyCode.Z) {

                            //If Key Pressed is the Same as Current Key Combination, Increment Tested Letter in Sequence
                            if (Input.GetKeyDown(key) && KeyCombination[CurrentTestedKey] == key.ToString()[0])
                            {

                                CurrentTestedKey++;
                            }
                            else if (Input.GetKeyDown(key))
                            {
                                //Else If Key Pressed is NOT the Same as Current Key Combination
                                Debug.Log("QTE Failed");

                                //Stop Timer
                                StopCoroutine(Timer);
                                Timer = null;

                                State = QTEState.Fail;                                   //Player failed

                            }
                        }
                    }
                }

                //If the Whole Sequence is Complete
                if (CurrentTestedKey >= NumKeys)
                {

                    Debug.Log("QTE Completed Successfully!");

                    State = QTEState.Success;                                   //Player was successful

                    //Stop Timer
                    StopCoroutine(Timer);
                    Timer = null;
                }
            }
            else if (QTEType == 1)
            {                                             //If the Type is Button Mash QTE

                //When Space is Pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CurrentClicks++;
                    Debug.Log("Space Pressed");
                }

                //If Required Number of Clicks is Reached
                if (CurrentClicks >= NumClicks)
                {

                    Debug.Log("QTE Completed Successfully!");

                    State = QTEState.Success;                                   //Player was successful

                    //Stop Timer
                    StopCoroutine(Timer);
                    Timer = null;
                }

            }
            else if (QTEType == 2)
            {

                //When Space is Pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    Debug.Log("Space Pressed at time " + CurrentTiming);

                    //If Pressed within the Timing Window
                    if (CurrentTiming >= RandomTiming && CurrentTiming <= (RandomTiming + 0.5f))
                    {

                        Debug.Log("QTE Completed Successfully!");

                        State = QTEState.Success;                                   //Player was successful

                        //Stop Timer
                        StopCoroutine(Timer);
                        Timer = null;
                    }
                    else
                    {
                        //Else If Not Pressed Within The Timing Window

                        Debug.Log("QTE Failed");

                        //Stop Timer
                        StopCoroutine(Timer);
                        Timer = null;

                        State = QTEState.Fail;                                   //Player failed
                    }
                } 
                else
                {
                    //When Space NOT Pressed, Increment Timer
                    if (DBDTimerStarted)
                    {
                        //Increment CurrentTiming
                        CurrentTiming += Time.deltaTime;
                    }
                    else
                    {
                        //Skip The First Frame To Avoid The Jump In Timing
                        DBDTimerStarted = true;
                    }
                }
            }
        }

        //QTE Timer
        IEnumerator TimerCoroutine(int Seconds)
        {

            //Start Timer
            yield return new WaitForSeconds(Seconds);

            //If Not Completed By Then
            if (Timer != null)
            {

                Timer = null;
                State = QTEState.Fail;                   //Player failed

                Debug.Log("QTE Failed");
            }
        }
    }
}
