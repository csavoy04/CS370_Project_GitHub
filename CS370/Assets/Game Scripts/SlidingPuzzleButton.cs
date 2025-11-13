using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidingPuzzleButton : MonoBehaviour
{
    [Header("Variables")]
    public string curScene;

    public Dictionary<string, float> locations = new Dictionary<string, float>()
    {
        // curScene, float
        {"World2", -100f}
    };

    public void Start() 
    {
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.name;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SlidingCube"))
        {
            Debug.Log("Puzzle Completed");
        }
    }

    void OnTriggerExit(Collider collision) 
    {
        Debug.Log("Puzzle Uncompleted");
    }
}
