using UnityEngine;

/*  Script made by: Coleman
        Script that:
            - Checks the slider puzzle for completion
            move the door
    Date: 11/23/2025
    Made in: C# VsCode
*/


public class PuzzleCompleted : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject button;
    public SlidingPuzzleButton script;

    void Awake()
    {
        script = button.GetComponent<SlidingPuzzleButton>();
    }

    void Update()
    {
        if (script.completed == true)
        {
            transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime;
        }
    }
}
