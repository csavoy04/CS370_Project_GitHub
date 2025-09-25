using System.Numerics;
using UnityEngine;

public enum BattleState {PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral}
public class combatHandler : MonoBehaviour
{

    public BattleState State;
    void Start(){

        State = BattleState.Neutral;
    }

    void Update(){
        
        if(State == BattleState.Start){
            
        }
    }
}
