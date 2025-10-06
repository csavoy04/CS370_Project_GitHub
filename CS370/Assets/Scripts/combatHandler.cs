using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral }

public class combatHandler : MonoBehaviour{
    public GameObject playerprefab;
    public GameObject enemyprefab;

    public BattleState State;

    void Start()
    {

        State = BattleState.Neutral;
        Debug.Log("Combat State Neutral");

    }

    void Update()
    {

        if (SceneManager.GetActiveScene().name == "CombatArea" && State == BattleState.Neutral)
        {
            State = BattleState.Start;
            Debug.Log("Combat State Start");
            battleStart();
        }

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
    public void PlayerTurn()
    {
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