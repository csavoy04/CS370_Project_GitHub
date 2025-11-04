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
        //Target Select
        else if (MState == "TargetSelect")
        {
            for (int i = 0; i < PartySystem.Instance.EnemyParty.Count; i++)
            {
                if (PartySystem.Instance.EnemyParty[i].IsAlive())
                {
                    if (i == 0)
                    {
                        if (PartySystem.Instance.EnemyParty[0].IsDead())
                        {
                            move0.text = "No Selection";
                        }
                        else
                        {
                            move0.text = PartySystem.Instance.EnemyParty[i].GetName();
                        }

                        move1.text = "No Selection";
                        move2.text = "No Selection";
                    }
                    else if (i == 1)
                    {
                        if (PartySystem.Instance.EnemyParty[0].IsDead())
                        {
                            move0.text = "No Selection";
                        }
                        else
                        {
                            move0.text = PartySystem.Instance.EnemyParty[0].GetName();
                        }

                        if (PartySystem.Instance.EnemyParty[1].IsDead())
                        {
                            move1.text = "No Selection";
                        }
                        else
                        {
                            move1.text = PartySystem.Instance.EnemyParty[1].GetName();
                        }
                        move2.text = "No Selection";
                    }
                    else if (i == 2)
                    {
                        if (PartySystem.Instance.EnemyParty[0].IsDead())
                        {
                            move0.text = "No Selection";
                        }
                        else
                        {
                            move0.text = PartySystem.Instance.EnemyParty[0].GetName();
                        }

                        if (PartySystem.Instance.EnemyParty[1].IsDead())
                        {
                            move1.text = "No Selection";
                        }
                        else
                        {
                            move1.text = PartySystem.Instance.EnemyParty[1].GetName();
                        }

                        if (PartySystem.Instance.EnemyParty[2].IsDead())
                        {
                            move2.text = "No Selection";
                        }
                        else
                        {
                            move2.text = PartySystem.Instance.EnemyParty[2].GetName();
                        }
                    }
                }
            }
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
           else if (MState == "TargetSelect")
           {
               combatHandler.EnemyTargetSelect(0);
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
        else if (MState == "TargetSelect")
        {
            combatHandler.EnemyTargetSelect(1);
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
        else if (MState == "TargetSelect")
        {
            combatHandler.EnemyTargetSelect(2);
        }
    }

    private void Move3Event(ClickEvent evt)
    {
        if (MState == "MoveSelect")
        {
            combatHandler.OpenMainMenu();
        }
        else if (MState == "TargetSelect")
        {
            combatHandler.OpenMoveSelectMenu();
        }
    }
}
