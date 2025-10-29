using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //Make visible in inspector

public class PartySystem : MonoBehaviour{

    //Party Lists

    public List<Unit> PlayerParty = null;
    public List<Unit> EnemyParty = null;
    public List<Unit> StoredParty = null;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        if (PlayerParty == null || PlayerParty.Count == 0){

            //Create Player Party
            PlayerParty = new List<Unit>();

            //PartyClass, Name, UnitClass, Level, Health, Mana, Attack, Defense, Speed, MoveSet

            //For Testing Purposes
            PlayerParty.Add(new Unit(Unit.PartyClass.Empty, "Empty", Unit.UnitClass.Empty, 0, 0, 0, 0, 0, 0, new string[] { "", "", "" }));
            //PlayerParty.Add(new Unit (Unit.PartyClass.Player, "Timmy",Unit.UnitClass.Warrior, 999, 999, 999, 999, 5, 999, new string[] { "Slash", "Shield Bash", "War Cry" }));
            PlayerParty.Add(new Unit(Unit.PartyClass.Player, "Steve", Unit.UnitClass.Mage, 10, 100, 100, 20, 5, 10, new string[] { "Fireball", "Ice Spike", "Lightning Bolt" }));
            PlayerParty.Add(new Unit(Unit.PartyClass.Empty, "Empty", Unit.UnitClass.Empty, 0, 0, 0, 0, 0, 0, new string[] { "", "", "" })); 
            //PlayerParty.Add(new Unit(Unit.PartyClass.Player, "Bob", Unit.UnitClass.Rogue, 5, 5, 5, 5, 5, 5, new string[] { "Backstab", "Poison Dart", "Vanish" }));
        }

        if (EnemyParty == null || EnemyParty.Count == 0){

            //Create Enemy Party
            EnemyParty = new List<Unit>();
        }

        if (StoredParty == null || StoredParty.Count == 0)
        {

            //Create Stored Party
            StoredParty = new List<Unit>();

            for (int i = 0; i < 16; i++)
            {
                StoredParty.Add(new Unit(Unit.PartyClass.Empty, "Empty", Unit.UnitClass.Empty, 0, 0, 0, 0, 0, 0, new string[] { "", "", "" }));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SwapPartyWithStorage(0, 5);
        } 
        else if (Input.GetKeyDown(KeyCode.U))
        {
            SwapPartyWithStorage(1, 5);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SwapPartyWithStorage(2, 6);
        }
        
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            SwapStorageWithStorage(5, 6);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            SwapPartyWithParty(1, 2);
        }
        */
    }

    public bool SwapPartyWithStorage(int CurrentPartySlot, int TargetStoredSlot)
    {
        int NumMembers = 0;
        foreach (Unit unit in PlayerParty)
        {
            if (unit.GetPartyClass() != "Empty")
            {
                NumMembers++;
            }
        }

        if (NumMembers == 1 && PlayerParty[CurrentPartySlot].GetPartyClass() != "Empty" && StoredParty[TargetStoredSlot].GetPartyClass() == "Empty")
        {
            Debug.Log("Swap Failed! You must have at least 1 member in your party!");
            return false;
        }
        else
        {
            Unit TempUnit = PlayerParty[CurrentPartySlot];
            PlayerParty[CurrentPartySlot] = StoredParty[TargetStoredSlot];
            StoredParty[TargetStoredSlot] = TempUnit;
            Debug.Log("Swap Success!");
            return true;
        }
    }

    public void SwapStorageWithStorage(int CurrentStoredPartySlot, int TargetStoredSlot)
    {
        Unit TempUnit = StoredParty[CurrentStoredPartySlot];
        StoredParty[CurrentStoredPartySlot] = StoredParty[TargetStoredSlot];
        StoredParty[TargetStoredSlot] = TempUnit;
        Debug.Log("Swap Success!");
    }

    public void SwapPartyWithParty(int CurrentPartySlot, int TargetPartySlot)
    {
        Unit TempUnit = PlayerParty[CurrentPartySlot];
        PlayerParty[CurrentPartySlot] = PlayerParty[TargetPartySlot];
        PlayerParty[TargetPartySlot] = TempUnit;
        Debug.Log("Swap Success!");
    }

}
