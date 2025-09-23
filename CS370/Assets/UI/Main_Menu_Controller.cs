using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Main_Menu_Controller : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        root = uiDocument.rootVisualElement;

        var playBtn = root.Q<VisualElement>("PlayBtn");
        playBtn.RegisterCallback<ClickEvent>(PlayEvent);

        var quitBtn = root.Q<VisualElement>("QuitBtn");
        quitBtn.RegisterCallback<ClickEvent>(QuitEvent);

    }

    // Updates per frame
    private void Update()
    {
        //closes the main menu with the M button
        if (Input.GetKeyDown(KeyCode.M))
        {
            
            if (root.style.display == DisplayStyle.None)
            {
                // Hide the menu
                root.style.display = DisplayStyle.Flex;
                Debug.Log("Menu opened");
            }
            else
            {
                // Show the menu
                root.style.display = DisplayStyle.None;
                Debug.Log("Menu closed");
            }
        }
    }

    private void PlayEvent(ClickEvent evt)
    {
        root.style.display = DisplayStyle.None;
    }

    private void QuitEvent(ClickEvent evt)
    {
        Debug.Log("Quitting Game");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
