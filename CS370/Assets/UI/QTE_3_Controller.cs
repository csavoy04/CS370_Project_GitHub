using UnityEngine;
using UnityEngine.UIElements;

public class QTE_3_Controller : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;

    private QTE_3 qte;
    public QuickTimeEvents quickTimeEvents;

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        qte = root.Q<QTE_3>();
        if (qte != null)
        {
            qte.dataSource = quickTimeEvents;
            HideQTE(); // hide on start
        }
    }

    void Update()
    {
        if (quickTimeEvents.QTEType == 1)
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
