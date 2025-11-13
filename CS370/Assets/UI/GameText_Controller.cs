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

    }

    void OnHover(string text1, string text2)
    {
        var name = root1.Q<Label>("Name");
        var actions = root1.Q<Label>("Text");
        name.text = text1;
        actions.text = text2;
        root1.style.visibility = Visibility.Visible;
    }

    void TryHover(int text1)
    {
        MState = combatHandler.GetMState();

        if (MState == "MoveSelect")
        {
            string moveName = combatHandler.GetCurrentUnitMove(text1);
            OnHover(moveName, "Power: " + "\nMana Cost: ");
        }
        else if (MState == "Main")
        {
            if (text1 == 0)
            {
                OnHover("Fight", "Select Moves.");
            }
            if (text1 == 1)
            {
                OnHover("Defend", "Recieve less damage and regenerates mana.");
            }
            if (text1 == 2)
            {
                OnHover("Flee", "Escape Battle.");
            }
        }
        else if (MState == "TargetSelect")
        {
            if (text1 == 0)
            {
                if (root2.Q<Button>("Move_0").text == "No Selection")
                {
                    OnHover("No Enemy to Select", "");
                }
                else
                {
                    OnHover(PartySystem.Instance.EnemyParty[0].GetName(), "Attack this enemy?");
                }
                
            }
            if (text1 == 1)
            {
                if (root2.Q<Button>("Move_1").text == "No Selection")
                {
                    OnHover("No Enemy to Select", "");
                }
                else
                {
                    OnHover(PartySystem.Instance.EnemyParty[1].GetName(), "Attack this enemy?");
                }
            }
            if (text1 == 2)
            {
                if (root2.Q<Button>("Move_2").text == "No Selection")
                {
                    OnHover("No Enemy to Select", "");
                }
                else
                {
                    OnHover(PartySystem.Instance.EnemyParty[2].GetName(), "Attack this enemy?");
                }
            }
        }
        else
        {
            return;
        }
        
    }

    void ClearInfo()
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
        else if (MState == "MoveSelect")
        {
            name.text = "Select Move";
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
}