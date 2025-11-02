using UnityEngine;
using UnityEngine.UIElements;

public class QTE_2_Controller : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;

    private QTE_2 qte;
    public QuickTimeEvents quickTimeEvents;

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        qte = root.Q<QTE_2>();
        if (qte != null)
        {
            qte.dataSource = quickTimeEvents;
            HideQTE(); // hide on start
        }
    }

    void Update()
    {
        if (quickTimeEvents.QTEType == 0)
        {
            ShowQTE();
            if (quickTimeEvents.State == QuickTimeEvents.QTEState.Success || quickTimeEvents.State == QuickTimeEvents.QTEState.Fail)
            {
                HideQTE();
            }
        }
    }

    public void ShowQTE()
    {
        if (qte != null)
        {
            qte.style.display = DisplayStyle.Flex;
        }
    }

    public void HideQTE()
    {
        if (qte != null)
        {
            qte.style.display = DisplayStyle.None;
        }
    }
}
