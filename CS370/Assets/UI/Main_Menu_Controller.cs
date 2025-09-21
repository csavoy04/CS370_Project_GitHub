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
        
        var quitBtn = root.Q<VisualElement>("QuitBtn");
        quitBtn.RegisterCallback<ClickEvent>(QuitEvent);

        var playBtn = root.Q<VisualElement>("PlayBtn");
        playBtn.RegisterCallback<ClickEvent>(PlayEvent);
    }

    // Update is called once per frame
    private void QuitEvent(ClickEvent evt)
    {
        Debug.Log("Quitting Game");
    }

    private void PlayEvent(ClickEvent evt)
    {
        gameObject.SetActive(false);
    }
}
