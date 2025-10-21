using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //Make visible in inspector

public class PartySystem : MonoBehaviour{

    //Party Lists

    public List<Unit> PlayerParty = null;
    public List<Unit> EnemyParty = null;

    public static PartySystem Instance;

    

    //Prevents duplicates and keeps between scenes
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // kill duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Debug.Log(Unit.UnitClass.Mage);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (PlayerParty == null || PlayerParty.Count == 0){

            //Create Player Party

            PlayerParty = new List<Unit>();

            //PartyClass, Name, UnitClass, Level, Health, Mana, Attack, Defense, Speed, MoveSet

            PlayerParty.Add(new Unit (Unit.PartyClass.Player, "Timmy",Unit.UnitClass.Warrior, 999, 999, 999, 999,999,999, new string[] { "Slash", "Shield Bash", "War Cry" }));
            PlayerParty.Add(new Unit (Unit.PartyClass.Player, "Steve", Unit.UnitClass.Mage, 10, 100, 100, 10,10,10, new string[] { "Fireball", "Ice Spike", "Lightning Bolt" }));
            PlayerParty.Add(new Unit (Unit.PartyClass.Player, "Bob", Unit.UnitClass.Rogue, 5, 5, 5, 5,5, 5, new string[] { "Backstab", "Poison Dart", "Vanish" }));

        }

        if (EnemyParty == null || EnemyParty.Count == 0){

            //Create Enemy Party

            EnemyParty = new List<Unit>();

            //PartyClass, Name, UnitClass, Level, Health, Mana, Attack, Defense, Speed, MoveSet

            EnemyParty.Add(new Unit (Unit.PartyClass.Enemy, "Slimmy", Unit.UnitClass.Slime, 1, 20, 20, 5,5,7, new string[] { "Tackle", "Bite", "Stomp" }));
            EnemyParty.Add(new Unit (Unit.PartyClass.Enemy, "Slimey", Unit.UnitClass.Slime, 1, 20, 20, 5,5,6, new string[] { "Tackle", "Bite", "Stomp" }));
            EnemyParty.Add(new Unit (Unit.PartyClass.Enemy, "Slim", Unit.UnitClass.Slime, 1, 20, 20, 5,5,5, new string[] { "Tackle", "Bite", "Stomp" }));

        }
    }

    // Update is called once per frame
    void Update(){

        if (Input.GetKeyDown(KeyCode.Q)){

            Debug.Log("Party Member 2 Name: " + PlayerParty[1].Name + " || Party Member 2 Lvl: " + PlayerParty[1].Level + " || Party Member 2 Type: " + PlayerParty[1].GetPartyClass());

        }
    }
}
