using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.SceneManagement;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Fled, DetermineTurn, CheckEnd, AnimationWait, QTEWait }
public enum MenuState { Main, MoveSelect, Defend, TargetSelect, Hide }


public class CombatHandler : MonoBehaviour
{

    public static CombatHandler Instance;

    public GameObject playerprefab;
    public GameObject enemyprefab;

    public QuickTimeEvents QTE;

    //Battle State
    public BattleState BState;

    //Menu State
    public MenuState MState;

    //Create Turn Order List
    public List<Unit> UnitBattleList = new List<Unit>();

    //Current Unit
    public Unit CurrentUnit;
    public int CurrentUnitIndex = 0;

    //Attacked Unit
    public Unit Defender;

    //Move Selected
    public string SelectedMove;

    void Start()
    {
        DetermineEnemyUnits();

        //Start Battle
        BState = BattleState.Start;
        Debug.Log("Combat BState Start");

        //Determine Initial Turn Order (Higher Speed First, If Player Speed == Enemy Speed, Player goes first)
        UnitBattleList.AddRange(PartySystem.Instance.PlayerParty);
        UnitBattleList.AddRange(PartySystem.Instance.EnemyParty);
        UnitBattleList.Sort((x, y) => y.GetSpeed().CompareTo(x.GetSpeed()));

        BattleStart();

        MState = MenuState.Hide;
    }

    void Update()
    {
        //Do Corresponding Turn Based on Order
        if (BState != BattleState.Won && BState != BattleState.Lost)
        {
            //Determine Whose Turn It Is
            if (BState == BattleState.DetermineTurn)
            {
                CurrentUnit = UnitBattleList[CurrentUnitIndex];

                    if (CurrentUnit.GetPartyClass() != "Empty")
                    {
                        if (CurrentUnit.GetCurrentHealth() > 0)
                        {
                            //Change BState Based on Current Unit's Party Class
                            if (CurrentUnit.GetPartyClass() == "Player")
                            {
                                BState = BattleState.PlayerTurn;
                                Debug.Log("Player Turn: " + CurrentUnit.GetName());
                                OpenMainMenu();

                            }
                            else if (CurrentUnit.GetPartyClass() == "Enemy")
                            {
                                BState = BattleState.EnemyTurn;
                                Debug.Log("Enemy Turn: " + CurrentUnit.GetName());
                                MState = MenuState.Hide;

                            }
                        }
                        else
                        {
                            Debug.Log(CurrentUnit.GetName() + " is defeated and cannot take a turn.");

                            CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                            BState = BattleState.DetermineTurn;
                        }
                    }
                    else
                    {
                        Debug.Log("Current Unit is Empty. Skipping turn.");
                        CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                        BState = BattleState.DetermineTurn;
                    }
            }

            //Execute Turn
            if (BState == BattleState.PlayerTurn || BState == BattleState.QTEWait)
            {
                PlayerTurn();

            }
            else if (BState == BattleState.EnemyTurn)
            {
                EnemyTurn();

            }

            //Check for Win/Loss Condition After Each Turn
            if (BState == BattleState.CheckEnd)
            {
                //Tracks Defeated Units
                int temp = 0;

                    //Tracks Number of Units in Player Party
                    int numParty = 0;
                foreach (Unit unit in PartySystem.Instance.PlayerParty)
                {
                    if (unit.GetPartyClass() != "Empty")
                    {
                        numParty++;
                    }
                }

                //For each unit in enemy party, check if health <= 0
                foreach (Unit unit in PartySystem.Instance.EnemyParty)
                {
                        //If conditions are met, increment temp
                        if (unit.GetCurrentHealth() <= 0)
                        {
                            temp++;
                        }
                }

                //If temp is equal to or greater than number of enemies, player wins. If Not, check for player defeat
                if (temp >= PartySystem.Instance.EnemyParty.Count)
                {
                    BState = BattleState.Won;
                }
                else
                {
                    //Tracks Defeated Units
                    temp = 0;

                    //For each unit in player party, check if health <= 0
                    foreach (Unit unit in PartySystem.Instance.PlayerParty)
                    {
                        //If both conditions are met, increment temp
                            if (unit.GetPartyClass() != "Empty" && unit.GetCurrentHealth() <= 0)
                            {
                                temp++;
                            }
                    }

                    //If temp is equal to or greater than number of players, player loses. If Not, continue battle
                    if (temp >= numParty)
                    {
                        BState = BattleState.Lost;
                    }
                    else
                    {
                        BState = BattleState.DetermineTurn;
                    }
                }
            }

            //If Battle is Won or Lost or Fled, End Battle
            if (BState == BattleState.Won || BState == BattleState.Lost || BState == BattleState.Fled)
            {
                BattleEnd();
            }
        }
    }

    //Battle Start Function
    public void BattleStart()
    {
        SpawnEnemy();
        SpawnPlayerTeam();

        BState = BattleState.DetermineTurn;
    }

    //Player Turn Function
    public void PlayerTurn()
    {
        if (MState == MenuState.Main)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                OpenMoveSelectMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                //Restore some mana for defending
                CurrentUnit.RestoreMana(5);

                Debug.Log(CurrentUnit.GetName() + " defended and restored some mana!");

                //End Turn
                CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                BState = BattleState.CheckEnd;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                BState = BattleState.Fled;
            }
        }
        else if (MState == MenuState.MoveSelect)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                OpenTargetSelectMenu(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                OpenTargetSelectMenu(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                OpenTargetSelectMenu(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                OpenMainMenu();
            }
        }
        else if (MState == MenuState.TargetSelect)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && PartySystem.Instance.EnemyParty[0].IsAlive())
            {
                Defender = PartySystem.Instance.EnemyParty[0];
                if (ExecuteMove(SelectedMove, CurrentUnit, Defender) == false)
                {
                    OpenMainMenu();
                }
                else
                {
                    MState = MenuState.Hide;
                }
            }
            else if (PartySystem.Instance.EnemyParty.Count >= 2 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && PartySystem.Instance.EnemyParty[1].IsAlive())
            {
                Defender = PartySystem.Instance.EnemyParty[1];
                if (ExecuteMove(SelectedMove, CurrentUnit, Defender) == false)
                {
                    OpenMainMenu();
                }
                else
                {
                    MState = MenuState.Hide;
                }
            }
            else if (PartySystem.Instance.EnemyParty.Count >= 3 && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && PartySystem.Instance.EnemyParty[2].IsAlive())
            {
                Defender = PartySystem.Instance.EnemyParty[2];
                if (ExecuteMove(SelectedMove, CurrentUnit, Defender) == false)
                {
                    OpenMainMenu();
                }
                else
                {
                    MState = MenuState.Hide;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                OpenMoveSelectMenu();
            }
        }
        else if (MState == MenuState.Hide && BState == BattleState.QTEWait)
        {
            if (QTE.State == QuickTimeEvents.QTEState.Success)
            {
                Debug.Log(CurrentUnit.GetName() + " used " + SelectedMove + " on " + Defender.GetName() + "!");

                CurrentUnit.DealDamage(Defender, SelectedMove);

                //Update Health Bars Here

                RunAnimation(SelectedMove);

            }
            else if (QTE.State == QuickTimeEvents.QTEState.Fail)
            {
                Debug.Log(CurrentUnit.GetName() + "'s " + SelectedMove + " missed!");

                RunAnimation(SelectedMove);

            }
        }
    }

    //Enemy Turn Function
    public void EnemyTurn()
    {
        //EnemyTargetOptions
        string Targets = "";

        //Tracks Number of Units in Player Party
        int numParty = 0;
        foreach (Unit unit in PartySystem.Instance.PlayerParty)
        {
            if (unit.GetPartyClass() != "Empty")
            {
                numParty++;
            }
        }

        for (int i = 0; i < PartySystem.Instance.PlayerParty.Count; i++)
        {
            if (PartySystem.Instance.PlayerParty[i].GetPartyClass() != "Empty" && PartySystem.Instance.PlayerParty[i].IsAlive())
            {
                Targets += i;
            }
        }

        int RandomTargetIndex = UnityEngine.Random.Range(0, Targets.Length);
        int RandomMoveIndex = UnityEngine.Random.Range(0, CurrentUnit.MoveSet.Length);

        int chosenTarget = (int)Char.GetNumericValue(Targets[RandomTargetIndex]);

        ExecuteMove(CurrentUnit.MoveSet[RandomMoveIndex], CurrentUnit, PartySystem.Instance.PlayerParty[chosenTarget]);
    }

    //Battle End Function
    public void BattleEnd()
    {

        if (BState == BattleState.Won)
        {
            //Tracks Exp Gain
            int TempExpGain = 0;
            int MoneyGain = 0;

            //For each unit in battle list, check if health <= 0 and party class is enemy
            foreach (Unit unit in PartySystem.Instance.EnemyParty)
            {
                //If conditions are met, increment temp
                if (unit.GetCurrentHealth() <= 0)
                {
                    TempExpGain += unit.GetLevel() * 10;                    //Exp Gain Formula: Enemy Level * 10
                    MoneyGain += unit.GetLevel() * 5;                       //Money Gain Formula: Enemy Level * 5
                }
            }

            //Add Money to Player
            GameHandler.Instance.Money += MoneyGain;

            //Distribute Exp to Player Party
            foreach (Unit unit in PartySystem.Instance.PlayerParty)
            {
                if (unit.GetPartyClass() != "Empty")
                {
                    unit.ResetStats(TempExpGain);
                }
            }

            Debug.Log("You won the battle!");
            SceneManager.LoadScene("TestArea");
        }
        else if (BState == BattleState.Lost)
        {
            Debug.Log("You lost the battle...");
            SceneManager.LoadScene("TestArea");
        }
        else if (BState == BattleState.Fled)
        {
            Debug.Log("You fled the battle.");
            SceneManager.LoadScene("TestArea");
        }
    }

    //Next Turn Function
    public int NextTurn(int TempCurrentUnitIndex)
    {
        TempCurrentUnitIndex++;
        if (TempCurrentUnitIndex >= UnitBattleList.Count)
        {
            //Update Turn Order Based on Current Speed Stats
            UnitBattleList.Sort((x, y) => y.GetSpeed().CompareTo(x.GetSpeed()));

            //Reset to First Unit
            TempCurrentUnitIndex = 0;
        }
        return TempCurrentUnitIndex;
    }

    //Execute Move Function Placeholder
    public bool ExecuteMove(string MoveName, Unit Attacker, Unit Defender)
    {

        if (Attacker.UseMana(MoveName))
        {
            //If Attacker is Enemy, skip QTE else start QTE
            if (Attacker.GetPartyClass() == "Enemy")
            {
                Debug.Log(Attacker.GetName() + " used " + MoveName + " on " + Defender.GetName() + "!");
                Attacker.DealDamage(Defender, MoveName);

                //End Turn for Enemy
                CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                BState = BattleState.CheckEnd;
                return true;
            }
            else
            {
                QTE.QTEType = Attacker.QuickTimeEventType(MoveName);
                QTE.State = QuickTimeEvents.QTEState.Start;

                MState = MenuState.Hide;
                BState = BattleState.QTEWait;

                return true;
            }
        }
        else
        {
            Debug.Log(Attacker.GetName() + " does not have enough mana to use " + MoveName + "!");

            if (Attacker.GetPartyClass() == "Enemy")
            {

                Attacker.RestoreMana(5);  //Restore some mana to enemy for next turn

                Debug.Log(Attacker.GetName() + " restored some mana!");

                //End Turn for Enemy if not enough mana
                CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                BState = BattleState.CheckEnd;
            }

            return false;
        }



    }

    //Run Animation Function Placeholder
    public void RunAnimation(string AnimationName)
    {
        int duration;

        switch (AnimationName)
        {
            case "Slash":
            case "Fireball":
            case "Backstab":
                duration = 2;
                break;
            case "Shield Bash":
            case "Ice Spike":
            case "Poison Dart":
                duration = 5;
                break;
            case "War Cry":
            case "Lightning Bolt":
            case "Vanish":
                duration = 3;
                break;
            default:
                duration = 1;
                break;
        }

        StartCoroutine(TimerCoroutine(duration));

        BState = BattleState.AnimationWait;

    }

    //Open Main Menu
    public void OpenMainMenu()
    {
        MState = MenuState.Main;
        Debug.Log("Player Menu Opened. Press 1 to Attack, 2 to Defend, and 3 to Flee.");
    }

    //Open Move Select Menu
    public void OpenMoveSelectMenu()
    {
        MState = MenuState.MoveSelect;
        Debug.Log("Move Select Menu Opened. Press 1 for " + CurrentUnit.MoveSet[0] + ", 2 for " + CurrentUnit.MoveSet[1] + ", 3 for " + CurrentUnit.MoveSet[2] + ", 4 to return to previous Menu");
    }

    //Open Target Select Menu
    public void OpenTargetSelectMenu(int MoveNumber)
    {

        //TargetDisplay String
        string TargetDisplay = "Target Select Menu Opened. Press ";

        SelectedMove = CurrentUnit.MoveSet[MoveNumber];
        MState = MenuState.TargetSelect;

        for (int i = 0; i < PartySystem.Instance.EnemyParty.Count; i++)
        {
            if (PartySystem.Instance.EnemyParty[i].IsAlive())
            {
                if (i == 0)
                {
                    TargetDisplay += "1 to target " + PartySystem.Instance.EnemyParty[i].GetName();
                }
                else if (i == 1 && PartySystem.Instance.EnemyParty[0].IsDead())
                {
                    TargetDisplay += "2 to target " + PartySystem.Instance.EnemyParty[i].GetName();
                }
                else if (i == 2 && PartySystem.Instance.EnemyParty[0].IsDead() && PartySystem.Instance.EnemyParty[1].IsDead())
                {
                    TargetDisplay += "3 to target " + PartySystem.Instance.EnemyParty[i].GetName();
                }
                else
                {
                    TargetDisplay += ", " + (i + 1) + " to target " + PartySystem.Instance.EnemyParty[i].GetName();
                }
            }
        }
        Debug.Log(TargetDisplay + ", 4 to return to previous Menu");
    }

    //QTE Timer
    IEnumerator TimerCoroutine(int Seconds)
    {

        //Start Timer
        yield return new WaitForSeconds(Seconds);

        //End Turn
        CurrentUnitIndex = NextTurn(CurrentUnitIndex);
        BState = BattleState.CheckEnd;

    }

    public void DetermineEnemyUnits()
    {
        int NoOfEnemies = UnityEngine.Random.Range(1, 4);       //(1 to 3 enemies)

        //Reset Enemy Party
        PartySystem.Instance.EnemyParty.Clear();

        //Determine Enemy Units Based on Area
        switch (GameHandler.Instance.CurrentCombatArea)
        {
            case GameHandler.CombatAreaName.SlimeField:
                for (int i = 0; i < NoOfEnemies; i++)
                {
                    string EnemyName;
                    int EnemyLevel = UnityEngine.Random.Range(1, 4);        //(Level 1 to 3 enemies)
                    int EnemyHealth = 20 + (EnemyLevel - 1) * 5;            //Health Scaling
                    int EnemyMana = 20 + (EnemyLevel - 1) * 5;              //Mana Scaling
                    int EnemyAttack = 10 + (EnemyLevel - 1) * 2;            //Attack Scaling
                    int EnemyDefense = 5 + (EnemyLevel - 1) * 2;            //Defense Scaling
                    int EnemySpeed = 5 + (EnemyLevel - 1) * 1;              //Speed Scaling

                    EnemyName = "Slime" + (i + 1);

                    //PartyClass, Name, UnitClass, Level, Health, Mana, Attack, Defense, Speed, MoveSet
                    PartySystem.Instance.EnemyParty.Add(new Unit(Unit.PartyClass.Enemy, EnemyName, Unit.UnitClass.Slime, EnemyLevel, EnemyHealth, EnemyMana, EnemyAttack, EnemyDefense, EnemySpeed, new string[] { "Tackle", "Bite", "Stomp" }));
                    //EnemyParty.Add(new Unit(Unit.PartyClass.Empty, "Empty", Unit.UnitClass.Empty, 0, 0, 0, 0, 0, 0, new string[] { "", "", "" }));
                }
                break;
            default:
                break;
        }
    }

    //Spawn Enemies Function
    public void SpawnEnemy()
    {

        int enemyCount = PartySystem.Instance.EnemyParty != null ? PartySystem.Instance.EnemyParty.Count : 0;
        for (int NoOfEnemies = 0; NoOfEnemies < enemyCount; NoOfEnemies++)
        {
            GameObject go = Instantiate(enemyprefab, new UnityEngine.Vector3(5, NoOfEnemies * 2, 0), UnityEngine.Quaternion.identity);
            // Try to find a FloatingHealthBar component on the instantiated prefab (or its children)
            HealthBar fhb = go.GetComponentInChildren<HealthBar>();
            if (fhb != null)
            {
                PartySystem.Instance.EnemyParty[NoOfEnemies].HealthBar = fhb;
            }
            else
            {
                Debug.LogWarning($"SpawnEnemy: spawned enemy prefab at index {NoOfEnemies} has no FloatingHealthBar component.");
            }
        }

    }

    //Spawn Player Team Function
    public void SpawnPlayerTeam()
    {

        int allyCount = PartySystem.Instance.PlayerParty != null ? PartySystem.Instance.PlayerParty.Count : 0;
        for (int NoOfAllies = 0; NoOfAllies < allyCount; NoOfAllies++)
        {
            GameObject go = Instantiate(playerprefab, new UnityEngine.Vector3(-5, NoOfAllies * 2, 0), UnityEngine.Quaternion.identity);
            HealthBar fhb = go.GetComponentInChildren<HealthBar>();
            if (fhb != null)
            {
                PartySystem.Instance.PlayerParty[NoOfAllies].HealthBar = fhb;
            }
            else
            {
                Debug.LogWarning($"SpawnPlayerTeam: spawned player prefab at index {NoOfAllies} has no FloatingHealthBar component.");
            }
        }
    }

    public string GetMState()
    {
        return MState.ToString();
    }

    public string GetCurrentUnitMove(int index)
    {
        return CurrentUnit.MoveSet[index];
    }

}

//Status effects can be added later as needed
//Examples: Poison, Burn, Freeze, Stun, Buffs/Debuffs, etc.
//Function for targeting allies for healing or buffs can also be added later
