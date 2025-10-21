using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, DetermineTurn, CheckEnd }
public enum MenuState { Main, MoveSelect, Defend, TargetSelect, Hide}

public class combatHandler : MonoBehaviour
{
    public GameObject playerprefab;
    public GameObject enemyprefab;

    //Battle State
    public BattleState BState;

    //Menu State
    public MenuState MState;

    //Create Turn Order List
    public List<Unit> UnitBattleList = new List<Unit>();

    //Current Unit
    public Unit CurrentUnit;
    public int CurrentUnitIndex = 0;

    //Target Unit
    public Unit TargetUnit;

    //Move Selected
    public string SelectedMove;

    void Start()
    {
        //Start Battle
        BState = BattleState.Start;
        Debug.Log("Combat BState Start");

        //Determine Initial Turn Order (If Player Speed == Enemy Speed, Player goes first)
        UnitBattleList.AddRange(PartySystem.Instance.PlayerParty);
        UnitBattleList.AddRange(PartySystem.Instance.EnemyParty);
        UnitBattleList.Sort((x, y) => y.Speed.CompareTo(x.Speed));

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

                if(CurrentUnit.Health > 0)
                {
                    //Change BState Based on Current Unit's Party Class
                    if (CurrentUnit.GetPartyClass() == "Player")
                    {
                        BState = BattleState.PlayerTurn;
                        Debug.Log("Player Turn: " + CurrentUnit.Name);
                        Debug.Log("Player Menu Opened. Press 1 to Attack, 2 to Defend, and 3 to Flee.");
                        MState = MenuState.Main;

                    }
                    else if (CurrentUnit.GetPartyClass() == "Enemy")
                    {
                        BState = BattleState.EnemyTurn;
                        Debug.Log("Enemy Turn: " + CurrentUnit.Name);

                    }
                } else
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
                MState = MenuState.Hide;
                EnemyTurn();
                
            }

            //Check for Win/Loss Condition After Each Turn
            if(BState == BattleState.CheckEnd)
            {
                //Tracks Defeated Units
                int temp = 0;

                //For each unit in battle list, check if health <= 0 and party class is enemy
                foreach (Unit unit in UnitBattleList)
                {
                    //If both conditions are met, increment temp
                    if (unit.Health <= 0 && unit.GetPartyClass() == "Enemy")
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
                        if (unit.Health <= 0 && unit.GetPartyClass() == "Player")
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

            //If Battle is Won or Lost, End Battle
            if (BState == BattleState.Won || BState == BattleState.Lost)
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
                MState = MenuState.MoveSelect;
                Debug.Log("Move Select Menu Opened. Press 1 for " + CurrentUnit.MoveSet[0] + ", 2 for " + CurrentUnit.MoveSet[1] + ", 3 for " + CurrentUnit.MoveSet[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log(CurrentUnit.Name + " Defended!");
                CurrentUnitIndex = NextTurn(CurrentUnitIndex);
                BState = BattleState.CheckEnd;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                Debug.Log("You fled the battle!");
                BState = BattleState.Lost;
            }
        }
        else if (MState == MenuState.MoveSelect)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SelectedMove = CurrentUnit.MoveSet[0];
                MState = MenuState.TargetSelect;
                Debug.Log("Target Select Menu Opened. Press 1 to target " + PartySystem.Instance.EnemyParty[0].Name + ", 2 for " + PartySystem.Instance.EnemyParty[1].Name + ", 3 for " + PartySystem.Instance.EnemyParty[2].Name + ", 4 to return to previous Menu");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SelectedMove = CurrentUnit.MoveSet[1];
                MState = MenuState.TargetSelect;
                Debug.Log("Target Select Menu Opened. Press 1 to target " + PartySystem.Instance.EnemyParty[0].Name + ", 2 for " + PartySystem.Instance.EnemyParty[1].Name + ", 3 for " + PartySystem.Instance.EnemyParty[2].Name + ", 4 to return to previous Menu");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SelectedMove = CurrentUnit.MoveSet[2];
                MState = MenuState.TargetSelect;
                Debug.Log("Target Select Menu Opened. Press 1 to target " + PartySystem.Instance.EnemyParty[0].Name + ", 2 for " + PartySystem.Instance.EnemyParty[1].Name + ", 3 for " + PartySystem.Instance.EnemyParty[2].Name + ", 4 to return to previous Menu");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                MState = MenuState.Main;
            }
        }
        else if (MState == MenuState.TargetSelect)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[0]);
                MState = MenuState.Hide;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[1]);
                MState = MenuState.Hide;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                ExecuteMove(SelectedMove, CurrentUnit, PartySystem.Instance.EnemyParty[2]);
                MState = MenuState.Hide;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                MState = MenuState.MoveSelect;
            }
        }
    }

    //Enemy Turn Function
    public void EnemyTurn()
    {
        Debug.Log(CurrentUnit.Name + " Did Something!");

        CurrentUnitIndex++;
        if (CurrentUnitIndex >= UnitBattleList.Count)
        {
            CurrentUnitIndex = 0;
        }
        BState = BattleState.CheckEnd;

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
    }

    //Spawn Enemies Function
    public void SpawnEnemy()
    {

        int NoOfEnemies = 3;

        for (NoOfEnemies = 0; NoOfEnemies < 3; NoOfEnemies++)
        {

            Instantiate(enemyprefab, new UnityEngine.Vector3(3, NoOfEnemies + 1, NoOfEnemies * 2), UnityEngine.Quaternion.identity);

        }

    }

    //Spawn Player Team Function
    public void SpawnPlayerTeam(){

        int NoOfAllies = 3;

        for (NoOfAllies = 0; NoOfAllies < 3; NoOfAllies++){

            Instantiate(playerprefab, new UnityEngine.Vector3(-3, NoOfAllies, NoOfAllies * 2), UnityEngine.Quaternion.identity);

        }
    }

    //Next Turn Function
    public int NextTurn(int TempCurrentUnitIndex)
    {
        TempCurrentUnitIndex++;
        if (TempCurrentUnitIndex >= UnitBattleList.Count)
        {
            TempCurrentUnitIndex = 0;
        }
        return TempCurrentUnitIndex;
    }

    //Execute Move Function Placeholder
    public void ExecuteMove(string MoveName, Unit Attacker, Unit Defender)
    {
        
        Debug.Log(Attacker.Name + " used " + MoveName + " on " + Defender.Name + "!");

        //End Player Turn
        CurrentUnitIndex = NextTurn(CurrentUnitIndex);
        BState = BattleState.CheckEnd;
            
    }

    //Run Animation Function Placeholder
    public void RunAnimation(string AnimationName)
    {
        //Placeholder for running animations
    }
}

//Solidify Execute Move function