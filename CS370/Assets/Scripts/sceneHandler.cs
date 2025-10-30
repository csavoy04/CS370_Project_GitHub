using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class sceneHandler : MonoBehaviour
{
    [Header("Scenes")]
    public static string curScene;
    public string tarScene;
    public string prevScene;

    [Header("Dictionary")]
    // The dictinary uses a concatanation of the current and previous scenes to dicate player spawn point
    public string key;
    public Vector3 pos;
    public Dictionary<string, Vector3> locations = new Dictionary<string, Vector3>()
    {
        // curScene | prevScene, Vector3
<<<<<<< Updated upstream
        {"TestAreaTestArea", new Vector3(-20f, 2f, 0f)},
        {"BlackSmithTestArea", new Vector3(0f, 0f, 0f)},
        {"TestAreaBlackSmith", new Vector3(-6.8f, 2f, 7f)}
=======
        {"World1BlackSmith", new Vector3(-6.8f, 2f, 7f)},
        {"World2World1", new Vector3(-18f, 2f, 0f)},
        {"World2BlackSmith", new Vector3(-4.4f, 2f, 7.3f)},
        {"World1World2", new Vector3(100f, 2f, 0f)}
>>>>>>> Stashed changes
    };

    public void Start()
    {
        prevScene = PlayerPrefs.GetString("prevScene");
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.name;
        if (prevScene != "")
        {   // Generate key (which concatanation of cur and prev scenes
            key = string.Concat(curScene, prevScene);
<<<<<<< Updated upstream
            pos = locations[key];
            transform.position = pos;
=======
            // If prev and cur scene are the same player is set to defualt location
            if (locations.ContainsKey(key))
            {
                pos = locations[key];
                transform.position = pos;
            }
>>>>>>> Stashed changes
        }
    }

    /*---------------------------------- SCENE TRIGGERS ----------------------------*/
    private void OnTriggerEnter(Collider collision)
<<<<<<< Updated upstream
    {
        tarScene = collision.gameObject.name;
=======
    {   // If the trigger is 'prevScene' (meaning its a building) then set tarScene to prevScene
        if (collision.gameObject.name == "prevScene")
        {
            tarScene = prevScene;
        }
        else
        {
            tarScene = collision.gameObject.name;
        }

>>>>>>> Stashed changes
        if (Application.CanStreamedLevelBeLoaded(tarScene))
        {   // Saves prevScene and loads new scene
            prevScene = curScene;
            PlayerPrefs.SetString("prevScene", prevScene);
            SceneManager.LoadScene(tarScene);
        }
    }
}
