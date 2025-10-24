using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.SceneManagement;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Fled, DetermineTurn, CheckEnd }
public enum MenuState { Main, MoveSelect, Defend, TargetSelect, Hide}


public class combatHandler : MonoBehaviour
{

    public event EventHandler OnHealthChanged;
    public GameObject playerprefab;
    public GameObject enemyprefab;

    public HealthBar healthBar;
    public Transform pfHealthBar;
    //Battle State
    public BattleState BState;

    //Menu State
    public MenuState MState;

    //Create Turn Order List
    public List<Unit> UnitBattleList = new List<Unit>();

    //Current Unit
    public Unit CurrentUnit;
    public int CurrentUnitIndex = 0;

    //Move Selected
    public string SelectedMove;

    void Awake()
    {
    }

    void Start()
    {
        //Start Battle
        BState = BattleState.Start;
        Debug.Log("Combat BState Start");

        //Determine Initial Turn Order (Higher Speed First, If Player Speed == Enemy Speed, Player goes first)
        UnitBattleList.AddRange(PartySystem.Instance.PlayerParty);
        UnitBattleList.AddRange(PartySystem.Instance.EnemyParty);
        UnitBattleList.Sort((x, y) => y.Speed.CompareTo(x.CurrentSpeed));

        battleStart();

        MState = MenuState.Hide;
    }

    void Update()
    {

        //Testing Battle End

        if (Input.GetKeyDown(KeyCode.V))
        {
            BState = BattleState.Won;
            BattleEnd();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BState = BattleState.Lost;
            BattleEnd();
        }


        //Do Corresponding Turn Based on Order
        if (BState != BattleState.Won && BState != BattleState.Lost)
        {
            //Determine Whose Turn It Is
            if (BState == BattleState.DetermineTurn)
            {
                CurrentUnit = UnitBattleList[CurrentUnitIndex];

                if (CurrentUnit.CurrentHealth > 0)
                {
                    //Change BState Based on Current Unit's Party Class
                    if (CurrentUnit.GetPartyClass() == "Player")
                    {
                        BState = BattleState.PlayerTurn;
                        Debug.Log("Player Turn: " + CurrentUnit.Name);
                        OpenMainMenu();

                    }
                    else if (CurrentUnit.GetPartyClass() == "Enemy")
                    {
                        BState = BattleState.EnemyTurn;
                        Debug.Log("Enemy Turn: " + CurrentUnit.Name);
                        MState = MenuState.Hide;

                    }
                }
                else
                {
                    Debug.Log(CurrentUnit.Name + " is defeated and cannot take a turn.");

                    CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                    BState = BattleState.DetermineTurn;
                }
            }

            //Execute Turn
            if (BState == BattleState.PlayerTurn)
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

                //For each unit in battle list, check if health <= 0 and party class is enemy
                foreach (Unit unit in UnitBattleList)
                {
                    //If both conditions are met, increment temp
                    if (unit.CurrentHealth <= 0 && unit.GetPartyClass() == "Enemy")
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

                    //For each unit in battle list, check if health <= 0 and party class is player
                    foreach (Unit unit in UnitBattleList)
                    {
                        //If both conditions are met, increment temp
                        if (unit.CurrentHealth <= 0 && unit.GetPartyClass() == "Player")
                        {
                            temp++;
                        }
                    }

                    //If temp is equal to or greater than number of players, player loses. If Not, continue battle
                    if (temp >= PartySystem.Instance.PlayerParty.Count)
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
    public void battleStart()
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
                Debug.Log(CurrentUnit.Name + " Defended!");
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
                if (ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[0]) == false)
                {
                    Debug.Log("Not enough mana to perform move. Returning to Main Menu.");
                    OpenMainMenu();
                }
            }
            else if (PartySystem.Instance.EnemyParty.Count >= 2 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && PartySystem.Instance.EnemyParty[1].IsAlive())
            {
                if (ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[1]) == false)
                {
                    Debug.Log("Not enough mana to perform move. Returning to Main Menu.");
                    OpenMainMenu();
                }
            }
            else if (PartySystem.Instance.EnemyParty.Count >= 3 && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && PartySystem.Instance.EnemyParty[2].IsAlive())
            {
                if (ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[2]) == false)
                {
                    Debug.Log("Not enough mana to perform move. Returning to Main Menu.");
                    OpenMainMenu();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                OpenMoveSelectMenu();
            }
        }
    }

    //Enemy Turn Function
    public void EnemyTurn()
    {
        //EnemyTargetOptions
        string Targets = "";
        for (int i = 0; i < PartySystem.Instance.PlayerParty.Count; i++)
        {
            if (PartySystem.Instance.PlayerParty[i].IsAlive())
            {
                Targets += i;
            }
        }

        int RandomTargetIndex = UnityEngine.Random.Range(0, Targets.Length);
        int RandomMoveIndex = UnityEngine.Random.Range(0, CurrentUnit.MoveSet.Length);

        ExecuteMove(CurrentUnit.MoveSet[RandomMoveIndex], CurrentUnit, PartySystem.Instance.PlayerParty[RandomTargetIndex]);
    }

    //Battle End Function
    public void BattleEnd()
    {
        if (BState == BattleState.Won)
        {
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


    //Spawn Enemies Function
    public void SpawnEnemy()
    {

        int NoOfEnemies = 3;

        for (NoOfEnemies = 0; NoOfEnemies < 3; NoOfEnemies++)
        {

            Instantiate(enemyprefab, new UnityEngine.Vector3(3, NoOfEnemies + 1, NoOfEnemies * 2), UnityEngine.Quaternion.identity);
            Transform HeathlBarTransform = Instantiate(pfHealthBar, new UnityEngine.Vector3(3, NoOfEnemies, NoOfEnemies), UnityEngine.Quaternion.identity);
            HealthBar healthBar = HeathlBarTransform.GetComponent<HealthBar>();

        }

    }

    //Spawn Player Team Function
    public void SpawnPlayerTeam()
    {

        int NoOfAllies = 3;

        for (NoOfAllies = 0; NoOfAllies < 3; NoOfAllies++)
        {

            Instantiate(playerprefab, new UnityEngine.Vector3(-3, NoOfAllies, NoOfAllies * 2), UnityEngine.Quaternion.identity);
            Transform HeathBarTransform = Instantiate(pfHealthBar, new UnityEngine.Vector3(-3, NoOfAllies, NoOfAllies), UnityEngine.Quaternion.identity);
            HealthBar healthBar = HeathBarTransform.GetComponent<HealthBar>();
        }
    }

    //Next Turn Function
    public int NextTurn(int TempCurrentUnitIndex)
    {
        TempCurrentUnitIndex++;
        if (TempCurrentUnitIndex >= UnitBattleList.Count)
        {
            //Update Turn Order Based on Current Speed Stats
            UnitBattleList.Sort((x, y) => y.Speed.CompareTo(x.CurrentSpeed));

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
            Debug.Log(Attacker.Name + " used " + MoveName + " on " + Defender.Name + "!");

            Debug.Log($"Before: {Defender.Name} HP = {Defender.CurrentHealth}");
            Attacker.DealDamage(Defender, MoveName);
            Debug.Log($"After: {Defender.Name} HP = {Defender.CurrentHealth}");

            Debug.Log($"ExecuteMove called with MoveName={MoveName}");
            Debug.Log($"Attacker={Attacker}, Defender={Defender}");
            if (Attacker != null) Debug.Log($"Attacker name: {Attacker.Name}");
            if (Defender != null) Debug.Log($"Defender name: {Defender.Name}");

            //Update Health Bars Here


            //End Turn
            CurrentUnitIndex = NextTurn(CurrentUnitIndex);
            BState = BattleState.CheckEnd;

            return true;
        }
        else
        {
            Debug.Log(Attacker.Name + " does not have enough mana to use " + MoveName + "!");

            return false;
        }



    }

    //Run Animation Function Placeholder
    public void RunAnimation(string AnimationName)
    {
        //Placeholder for running animations
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
                    TargetDisplay += "1 to target " + PartySystem.Instance.EnemyParty[i].Name;
                }
                else if (i == 1 && PartySystem.Instance.EnemyParty[0].IsDead())
                {
                    TargetDisplay += "2 to target " + PartySystem.Instance.EnemyParty[i].Name;
                }
                else if (i == 2 && PartySystem.Instance.EnemyParty[0].IsDead() && PartySystem.Instance.EnemyParty[1].IsDead())
                {
                    TargetDisplay += "3 to target " + PartySystem.Instance.EnemyParty[i].Name;
                }
                else
                {
                    TargetDisplay += ", " + (i + 1) + " to target " + PartySystem.Instance.EnemyParty[i].Name;
                }
            }
        }
        Debug.Log(TargetDisplay + ", 4 to return to previous Menu");
    }

    

//Add exp after battle
//Add money after battle
//Add delay between turns
//Add mana cost for moves
}

