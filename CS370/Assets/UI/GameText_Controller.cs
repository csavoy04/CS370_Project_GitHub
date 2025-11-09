using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameText_Controller : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument1;
    [SerializeField] UIDocument uiDocument2;

    private VisualElement root1;
    private VisualElement root2;

    public CombatHandler combatHandler;
    public string MState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        root1 = uiDocument1.rootVisualElement;
        root2 = uiDocument2.rootVisualElement;

        var buttonA = root2.Q<Button>("Move_0");
        var buttonB = root2.Q<Button>("Move_1");
        var buttonC = root2.Q<Button>("Move_2");

        if (buttonA != null)
        {
            buttonA.RegisterCallback<MouseEnterEvent>(evt => TryHover(0));
            buttonA.RegisterCallback<MouseLeaveEvent>(evt => ClearInfo());
        }

        if (buttonB != null)
        {
            buttonB.RegisterCallback<MouseEnterEvent>(evt => TryHover(1));
            buttonB.RegisterCallback<MouseLeaveEvent>(evt => ClearInfo());
        }

        if (buttonC != null)
        {
            buttonC.RegisterCallback<MouseEnterEvent>(evt => TryHover(2));
            buttonC.RegisterCallback<MouseLeaveEvent>(evt => ClearInfo());
        }
    }

    // Updates per frame
    private void Update()
    {
        var name = root1.Q<Label>("Name");
        var text = root1.Q<Label>("Text");

        MState = combatHandler.GetMState();

        if (MState == "Main")
        {
            name.text = combatHandler.GetCurrentUnitName() + "'s Turn";
            text.text = "";
            root1.style.visibility = Visibility.Visible;
        }
        else if (MState == "TargetSelect")
        {
            name.text = "Select Enemy";
            text.text = "";
            root1.style.visibility = Visibility.Visible;
        }
    }

    void OnHover(string text1, string text2)
    {
        var name = root1.Q<Label>("Name");
        var actions = root1.Q<Label>("Text");
        name.text = text1;
        actions.text = text2;
        root1.style.visibility = Visibility.Visible;
    }

    void TryHover(int moveIndex)
    {
        MState = combatHandler.GetMState();

        if (MState != "MoveSelect")
        {
            return;
        }

        string moveName = combatHandler.GetCurrentUnitMove(moveIndex);
        OnHover(moveName, "Power:" + "\nMana Cost:");
    }

    void ClearInfo()
    {
        root1.style.visibility = Visibility.Hidden;
    }
}