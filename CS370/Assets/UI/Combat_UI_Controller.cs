using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Combat_UI_Controller : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;

    public CombatHandler combatHandler;
    public BattleState BState;
    public string MState;


    
    void Start()
    {
        root = uiDocument.rootVisualElement;


    }

    // Update is called once per frame
    void Update()
    {

        MState = combatHandler.GetMState();
        
        var move0 = root.Q<Button>("Move_0");
        move0.RegisterCallback<ClickEvent>(Move0Event);

        var move1 = root.Q<Button>("Move_1");
        move1.RegisterCallback<ClickEvent>(Move1Event);

        var move2 = root.Q<Button>("Move_2");
        move2.RegisterCallback<ClickEvent>(Move2Event);

        var move3 = root.Q<Button>("Move_3");
        move3.RegisterCallback<ClickEvent>(Move3Event);
        

        //Main Battle Text
        if (MState == "Main")
        {
            move0.text = "Attack";
            move1.text = "Defend";
            move2.text = "Flee";
            move3.text = "IDK";
        }
        //Selection of moves
        else if (MState == "MoveSelect")
        {
            move0.text = combatHandler.GetCurrentUnitMove(0);
            move1.text = combatHandler.GetCurrentUnitMove(1);
            move2.text = combatHandler.GetCurrentUnitMove(2);
            move3.text = "Back";
        }
        
    }

    private void Move0Event(ClickEvent evt)
    {
        if (MState == "Main")
        {
            combatHandler.OpenMoveSelectMenu();
        }
        else if (MState == "MoveSelect")
        {
            combatHandler.OpenTargetSelectMenu(0);
        }
    }

    private void Move1Event(ClickEvent evt)
    {
        if (MState == "Main")
        {
            combatHandler.OpenMoveSelectMenu();
        }
        else if (MState == "MoveSelect")
        {
            combatHandler.OpenTargetSelectMenu(1);
        }
    }

    private void Move2Event(ClickEvent evt)
    {
        if (MState == "Main")
        {
            combatHandler.OpenMoveSelectMenu();
        }
        else if (MState == "MoveSelect")
        {
            combatHandler.OpenTargetSelectMenu(2);
        }
    }

    private void Move3Event(ClickEvent evt)
    {
        if (MState == "MoveSelect")
        {
            combatHandler.OpenMainMenu();
        }

    }
}
