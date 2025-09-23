using UnityEngine;

public class quickTimeEvents : MonoBehaviour
{
    int QuickTimeEventType;
    bool Generate;
    int CurrentTestedKey;
    bool Success;
    bool Timer;

    int NumKeys = 5;
    char[NumKeys] KeyCombination;
    int RandomNum;
    string Characters = "abcdefghijklmnopqrstuvwxyz";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuickTimeEventType = 0;
        Generate = true;
        CurrentTestedKey = 0;
        Success = false;
        Timer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Start Generation
        if (Generate == true)
        {
            Debug.Log("Generation Started");

            //If Key Combination QTE
            if (QuickTimeEventType == 1)
            {
                //Generate Key Combination
                for (int i = 0; i < NumKeys; i++)
                {
                    RandomNum = random.Next(1, 27);
                    KeyCombination[i] = Characters[RandomNum];
                }
            }

            //Stop Generation
            Generate = false;
            StartCoroutine(TimerCoroutine(10));
            Debug.Log("Generation Stopped");
        }

        //Test Input
        if (QuickTimeEventType == 1 && Timer == true)
        {
            //Check if Key Pressed Matches KeyCombination
            if (Input.GetKeyDown(KeyCode.KeyCombination[CurrentTestedKey]) == true)
            {
                CurrentTestedKey++;
            }
            if (CurrentTestedKey >= NumKeys)
            {
                Success = true;
                Timer = false;
            }
        }
    }

    IEnumerator TimerCoroutine(int seconds)
    {
        Timer = true;

        yield return new WaitForSeconds(seconds);
        if (QuickTimeEventType == 1)
        {
            if (Timer == true && Success == false)
            {
                Timer = false;
            }
            else if (TimerCoroutine == false && Success == true)
            {
                
            }
        }
    }
}
