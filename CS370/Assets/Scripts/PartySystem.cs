using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //Make visible in inspector

public class PartySystem : MonoBehaviour{

    //Party Lists
    public List<Character> PlayerParty = null;
    public List<Enemy> EnemyParty = null;

    public static PartySystem Instance;

    //Prevents duplicates and keeps between scenes
    void Awake(){

        if (Instance != null && Instance != this){
            Destroy(gameObject); // kill duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (PlayerParty == null || PlayerParty.Count == 0){

            //Create Player Party
            PlayerParty = new List<Character>();

            PlayerParty.Add(new Character("Timmy", 999, 999, 999, 999,999,999));
            PlayerParty.Add(new Character("Steve", 10, 100, 100, 10,10,10));
            PlayerParty.Add(new Character("Bob", 5, 5, 5, 5,5, 5));

        }

        if (EnemyParty == null || EnemyParty.Count == 0){

            //Create Enemy Party
            EnemyParty = new List<Enemy>();

            EnemyParty.Add(new Enemy("Slimmy", 1, 20, 20, 5,5,5));
            EnemyParty.Add(new Enemy("Slimey", 1, 20, 20, 5,5,5));
            EnemyParty.Add(new Enemy("Slim", 1, 20, 20, 5,5,5));
        }
    }

    // Update is called once per frame
    void Update(){

        if (Input.GetKeyDown(KeyCode.V)){

            Debug.Log("Party Member 2 Name: " + PlayerParty[1].Name + " || Party Member 2 Lvl: " + PlayerParty[1].Level);

        }
    }
}
