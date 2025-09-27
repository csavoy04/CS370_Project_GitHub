using System.Numerics;
using UnityEngine;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral }

public class combatHandler : MonoBehaviour
{
    public GameObject playerprefab;
    public GameObject enemyprefab;
    public BattleState State;
    void Start(){

        State = BattleState.Neutral;
    }

    void Update(){

        if (State == BattleState.Start)
        {
            Instantiate(playerprefab, new UnityEngine.Vector3(-3, 0, 0), UnityEngine.Quaternion.identity);
            Instantiate(enemyprefab, new UnityEngine.Vector3(3, 0, 0), UnityEngine.Quaternion.identity);
        }
    }
}
