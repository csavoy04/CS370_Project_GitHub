using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/*  Script made by: Coolman Savvy
        Script that:
            - Scene handler that allows the player
            to move between scenes
            - Uses a dictionary to properly place
            the player based off prev and cur scenes
    Date: 10/30/2025
    Made in: C# VsCode
*/
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
        {"TestAreaBlackSmith", new Vector3(-6.8f, 2f, 7f)},
        {"World2TestArea", new Vector3(-18f, 2f, 0f)},
        {"World2BlackSmith", new Vector3(-4.4f, 2f, 7.3f)},
        {"TestAreaWorld2", new Vector3(100f, 2f, 0f)},
        {"World3World2", new Vector3(-20f, 2f, 0f)},
        {"World2World3", new Vector3(158f, 2f, 0f)},
        {"World3World4", new Vector3(104f, 2f, 0f)},
        {"World4World3", new Vector3(-20f, 2f, 0f)},
        {"World4BlackSmith", new Vector3(-6.8f, 2f, 7f)}
    };

    public void Start()
    {
        prevScene = PlayerPrefs.GetString("prevScene");
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.name;
        if (prevScene != "")
        {
            key = string.Concat(curScene, prevScene);
            // If prev and cur scene are the same player is set to defualt location
            if (locations.ContainsKey(key))
            {
                pos = locations[key];
                transform.position = pos;
            }
        }
    }

    /*---------------------------------- SCENE TRIGGERS ----------------------------*/
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "prevScene")
        {
            tarScene = prevScene;
        }
        else
        {
            tarScene = collision.gameObject.name;
        }

        if (Application.CanStreamedLevelBeLoaded(tarScene))
        {
            prevScene = curScene;

            // Saving data
            PlayerPrefs.SetString("prevScene", prevScene);

            SceneManager.LoadScene(tarScene);
        }
    }
}
