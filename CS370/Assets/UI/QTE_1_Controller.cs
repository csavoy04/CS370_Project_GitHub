using UnityEngine;
using UnityEngine.UIElements;

public class QTE_1_Controller : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;

    public QuickTimeEvents quickTimeEvents;

    private void OnEnable()
    {
        root.Q<QTE_1>().dataSource = quickTimeEvents;
    }
}
