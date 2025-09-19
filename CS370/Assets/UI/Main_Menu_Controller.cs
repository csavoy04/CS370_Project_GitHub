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
        quitBtn.RegisterCallback<ClickEvent>(OnClickEvent);

    }

    // Update is called once per frame
    private void OnClickEvent(ClickEvent evt)
    {
        Debug.Log("Quitting Game");
    }
}
