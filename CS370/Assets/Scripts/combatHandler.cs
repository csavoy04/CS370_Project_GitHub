using System.Numerics;
using UnityEngine;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral }

public class combatHandler : MonoBehaviour
{
    /*
    public GameObject playerprefab;
    public GameObject enemyprefab;
    public BattleState State;
    void Start()
    {
        State = BattleState.Neutral;
        Debug.Log("Combat State Neutral");
        State = BattleState.Start;
        Debug.Log("Combat State Start");
    }

    void Update()
    {
        if (State == BattleState.Start)
        {
            SpawnEnemy();
            SpawnPlayerTeam();
            PlayerTurn();
        }
    }
    public void PlayerTurn()
    {
        Input.GetKeyDown(KeyCode.V);
        Debug.Log("Pressed V");
        State = BattleState.EnemyTurn;
        Debug.Log("Combat State Enemy Turn");
    }
    public void SpawnEnemy()
    {
        int NoOfEnemies = 3;
        for (NoOfEnemies = 0; NoOfEnemies < 3; NoOfEnemies++)
        {
            Instantiate(enemyprefab, new UnityEngine.Vector3(3, NoOfEnemies + 1, NoOfEnemies*2), UnityEngine.Quaternion.identity);
        }

    }
    public void SpawnPlayerTeam()
    {
        int NoOfAllies = 3;
        for (NoOfAllies = 0; NoOfAllies < 3; NoOfAllies++)
        {
            Instantiate(playerprefab, new UnityEngine.Vector3(-3, NoOfAllies , NoOfAllies*2), UnityEngine.Quaternion.identity);
        }
    } 
    */
}
