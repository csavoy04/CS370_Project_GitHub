using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost }

public class combatHandler : MonoBehaviour{
    public GameObject playerprefab;
    public GameObject enemyprefab;

    //Battle State
    public BattleState State;

    //Create Turn Order List
    public List<Unit> UnitBattleList = new List<Unit>();

    void Start()
    {

        //Start Battle

        //State = BattleState.Neutral;
        State = BattleState.Start;
        //Debug.Log("Combat State Neutral");
        Debug.Log("Combat State Start");

        //Determine Initial Turn Order (If Player Speed == Enemy Speed, Player goes first)
        UnitBattleList.AddRange(PartySystem.Instance.PlayerParty);
        UnitBattleList.AddRange(PartySystem.Instance.EnemyParty);
        UnitBattleList.Sort((x, y) => y.Speed.CompareTo(x.Speed));

        battleStart();

}

    void Update()
    {
        /*
        if (SceneManager.GetActiveScene().name == "CombatArea" && State == BattleState.Neutral)
        {
            State = BattleState.Start;
            Debug.Log("Combat State Start");
            battleStart();
        }
        */

        //Testing Battle End

        if (Input.GetKeyDown(KeyCode.V))
        {
            State = BattleState.Won;
            BattleEnd();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            State = BattleState.Lost;
            BattleEnd();
        }

        //Do Corresponding Turn Based on Order

        //Check for Win/Loss Condition

        //Update Turn Order
        //UnitBattleList.Sort((x, y) => y.Speed.CompareTo(x.Speed));

    }

    public void battleStart()
    {
        SpawnEnemy();
        SpawnPlayerTeam();
        PlayerTurn();
    }

    public void BattleEnd()
    {
        if (State == BattleState.Won)
        {
            Debug.Log("You won the battle!");
            SceneManager.LoadScene("TestArea");
        }
        else if (State == BattleState.Lost)
        {
            Debug.Log("You lost the battle...");
            SceneManager.LoadScene("TestArea");
        }
    }

    public void PlayerTurn(){

        State = BattleState.PlayerTurn;

    }

    public void SpawnEnemy(){

        int NoOfEnemies = 3;

        for (NoOfEnemies = 0; NoOfEnemies < 3; NoOfEnemies++){

            Instantiate(enemyprefab, new UnityEngine.Vector3(3, NoOfEnemies + 1, NoOfEnemies * 2), UnityEngine.Quaternion.identity);

        }

    }

    public void SpawnPlayerTeam(){

        int NoOfAllies = 3;

        for (NoOfAllies = 0; NoOfAllies < 3; NoOfAllies++){

            Instantiate(playerprefab, new UnityEngine.Vector3(-3, NoOfAllies, NoOfAllies * 2), UnityEngine.Quaternion.identity);

        }
    } 
}