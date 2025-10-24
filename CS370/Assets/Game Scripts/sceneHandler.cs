using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneHandler : MonoBehaviour
{
    [Header("Scenes")]
    public string curScene;
    public string tarScene;
    public string prevScene;

    public void Start()
    {
        tarScene = PlayerPrefs.GetString("tarScene");
        prevScene = PlayerPrefs.GetString("prevScene");
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.name;
    }

    /*---------------------------------- SCENE TRIGGERS ----------------------------*/
    private void OnTriggerEnter(Collider collision)
    {
        tarScene = collision.gameObject.name;
        prevScene = curScene;
        PlayerPrefs.SetString("tarScene", tarScene);
        PlayerPrefs.SetString("prevScene", prevScene);
        SceneManager.LoadScene(tarScene);
    }
}
