using System.Collections.Generic;
using UnityEngine;
//using static CharacterBase;
//using static EnemyBase;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral }

public class combatHandler : MonoBehaviour
{
    public GameObject playerprefab;
    public GameObject enemyprefab;
    public BattleState State;
    public List<Character> PlayerParty;
    public List<Enemy> EnemyParty;


    void Start()
    {
        State = BattleState.Neutral;
        Debug.Log("Combat State Neutral");
        State = BattleState.Start;
        Debug.Log("Combat State Start");

        //Create Player Party (for testing)
        PlayerParty = new List<Character>();

        PlayerParty.Add(new Character("Timmy", 999, 999, 999, 999));
        PlayerParty.Add(new Character("Steve", 10, 100, 100, 10));
        PlayerParty.Add(new Character("Bob", 5, 5, 5, 5));

        Debug.Log("Party Member 2 Name: " + PlayerParty[1].Name + " || Party Member 2 Lvl: " + PlayerParty[1].Level);

        //Create Enemy Party (for testing)
        EnemyParty = new List<Enemy>();

        EnemyParty.Add(new Enemy("Slimmy", 1, 20, 20, 5));
        EnemyParty.Add(new Enemy("Slimey", 1, 20, 20, 5));
        EnemyParty.Add(new Enemy("Slim", 1, 20, 20, 5));
    }
    
    void Update(){
        if (State == BattleState.Start){
            SpawnEnemy();
            SpawnPlayerTeam();
            PlayerTurn();
        }
    }
    public void PlayerTurn(){

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Pressed V");
        }
            
        State = BattleState.EnemyTurn;
        Debug.Log("Combat State Enemy Turn");
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




