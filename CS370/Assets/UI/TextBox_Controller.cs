using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TextBox_Controller : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        root = uiDocument.rootVisualElement;

        //Closes Party Menu at start
        var name = root.Q<Label>("Name");
        name.text = "Player";

        var text = root.Q<Label>("Text");
        text.text = "I am the best!";

    }

    // Updates per frame
    private void Update()
    {
        
    }


}