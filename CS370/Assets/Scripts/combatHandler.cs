using System.Numerics;
using UnityEngine;

public enum BattleState { PlayerTurn, EnemyTurn, Start, Won, Lost, Neutral }

public class combatHandler : MonoBehaviour
{
    /*
    public GameObject playerprefab;
    public GameObject enemyprefab;
    public BattleState State;

    //Party List
    public List<Character> PlayerParty;
    void Start()
    {
        State = BattleState.Neutral;
        Debug.Log("Combat State Neutral");
        State = BattleState.Start;
        Debug.Log("Combat State Start");

        //Create Party (for testing)
        PlayerParty = new List<Character>();

        PlayerParty.Add(new Character("Timmy", 999, 999, 999, 999));
        PlayerParty.Add(new Character("Steve", 10, 100, 100, 10));
        PlayerParty.Add(new Character("Bob", 5, 5, 5, 5));

        Debug.Log("Party Member 2 Name: " + PlayerParty[1].Name + " || Party Member 2 Lvl: " + PlayerParty[1].Level);
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
            Instantiate(enemyprefab, new UnityEngine.Vector3(3, NoOfEnemies + 1, NoOfEnemies * 2), UnityEngine.Quaternion.identity);
        }

    }
    public void SpawnPlayerTeam()
    {
        int NoOfAllies = 3;
        for (NoOfAllies = 0; NoOfAllies < 3; NoOfAllies++)
        {
            Instantiate(playerprefab, new UnityEngine.Vector3(-3, NoOfAllies, NoOfAllies * 2), UnityEngine.Quaternion.identity);
        }
    } 
    */
}

//Party System
public struct Character
{
    //Stats
    public string Name { get; set; }
    public int Level { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }

    //Contructor
    public Character(string GivenName, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack)
    {
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
    }
}

