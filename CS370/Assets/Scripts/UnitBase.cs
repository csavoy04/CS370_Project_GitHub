using System;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public HealthBar HealthBar;
    //Stats
    public PartyClass PartyType;        // Party Type (Player or Enemy)
    public string Name;                 // Unit Name
    public UnitClass ClassType;         // Unit Class Type
    public int Level;                   // Current Level

    public int CurrentHealth;           // Current Health stat
    public int MaxHealth;               // Maximum Health stat

    public int CurrentMana;             // Current Mana stat
    public int MaxMana;                 // Maximum Mana stat

    public int Attack;                  // Base Attack stat
    public int CurrentAttack;           // Modified Attack stat during battle

    public int Defense;                 // Base Defense stat
    public int CurrentDefense;          // Modified Defense stat during battle

    public int Speed;                   // Base Speed stat
    public int CurrentSpeed;            // Modified Speed stat during battle

    public int Experience;              // Current Experience Points
    public int MaxExperience;           // Experience Points needed for next level

    //Armor? (Phyiscal, Magical)
    //Resistances? (Fire, Ice, Lightning, Poison, etc.)

    public int CurrentCritChance;
    public int CritChance;

    public int CurrentDodgeChance;
    public int DodgeChance;

    public int CurrentAccuracy;
    public int Accuracy;

    //Scaling Stats? (Physical/Magical)

    public string[] MoveSet;

    public enum PartyClass
    {
        Player,
        Enemy,
        Empty
    }

    public enum UnitClass
    {
        Warrior,
        Mage,
        Rogue,
        Slime,
        Goblin,
        Orc,
        Empty
    }

    public Unit (PartyClass GivenPartyType, string GivenName, UnitClass GivenClassType, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed, int GivenCritChance, int GivenDodgeChance, int GivenAccuracy, string[] GivenMoveSet)
    {
        PartyType = GivenPartyType;
        Name = GivenName;
        ClassType = GivenClassType;
        Level = GivenLevel;

        CurrentHealth = GivenHealth;
        MaxHealth = CurrentHealth;

        CurrentMana = GivenMana;
        MaxMana = CurrentMana;

        Attack = GivenAttack;
        CurrentAttack = GivenAttack;

        Defense = GivenDefense;
        CurrentDefense = GivenDefense;

        Speed = GivenSpeed;
        CurrentSpeed = GivenSpeed;

        CritChance = GivenCritChance;
        CurrentCritChance = GivenCritChance;

        DodgeChance = GivenDodgeChance;
        CurrentDodgeChance = GivenDodgeChance;

        Accuracy = GivenAccuracy;
        CurrentAccuracy = GivenAccuracy;

        Experience = 0;
        MaxExperience = Level * 100;

        MoveSet = GivenMoveSet;
    }

    public void LevelUp()
    {
        Level += 1;
        MaxHealth += 10;
        MaxMana += 5;
        Attack += 2;
        Defense += 2;
        Speed += 1;
        MaxExperience = Level * 100;
    }

    public void AddCharacterAttacks(string newMove, int placement)
    {
        if (placement >= 0 && placement < MoveSet.Length)
        {
            MoveSet[placement] = newMove;
        }
    }

    public void RemoveCharacterAttacks(int placement)
    {
        if (placement >= 0 && placement < MoveSet.Length)
        {
            MoveSet[placement] = null;
        }
    }

    public void ClearMoveSet()
    {
        for (int i = 0; i < MoveSet.Length; i++)
        {
            MoveSet[i] = null;
        }
    }

    public void DisplayMoveSet()
    {
        Debug.Log($"MoveSet for {Name}: {string.Join(", ", MoveSet)}");
    }

    // Prints the unit's stats to the console
    public void DisplayStats()
    {
        Debug.Log($"PartyType: {PartyType}, Name: {Name}, ClassType: {ClassType} Level: {Level}, MaxHealth: {MaxHealth}, MaxMana: {MaxMana}, Attack: {Attack}, Defense: {Defense}, Speed: {Speed}, Experience: {Experience} / {MaxExperience}");
    }

    //Gets the attack stat
    public int GetAttack()
    {
        return CurrentAttack;
    }

    public int GetBaseAttack()
    {
        return Attack;
    }

    public int GetDefense()
    {
        return CurrentDefense;
    }

    public int GetBaseDefense()
    {
        return Defense;
    }

    public int GetSpeed()
    {
        return CurrentSpeed;
    }

    public int GetBaseSpeed()
    {
        return Speed;
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public float GetHealthPercent()
    {
        return (float)CurrentHealth / MaxHealth;
    }

    public int GetCurrentMana()
    {
        return CurrentMana;
    }

    public int GetMaxMana()
    {
        return MaxMana;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetUnitClass()
    {
        return ClassType.ToString();
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetExperience()
    {
        return Experience;
    }

    public int GetMaxExperience()
    {
        return MaxExperience;
    }

    public int GetCurrentCritChance()
    {
        return CurrentCritChance;
    }

    public int GetCritChance()
    {
        return CritChance;
    }

    public int GetCurrentDodgeChance()
    {
        return CurrentDodgeChance;
    }

    public int GetDodgeChance()
    {
        return DodgeChance;
    }

    public int GetCurrentAccuracy()
    {
        return CurrentAccuracy;
    }

    public int GetAccuracy()
    {
        return Accuracy;
    }

    public string[] GetMoveSet()
    {
        return MoveSet;
    }

    // Returns the party class as a string
    public string GetPartyClass()
    {
        return PartyType.ToString();
    }

    // Heals the unit, increasing current health
    public void Heal(int Amount)
    {
        CurrentHealth += Amount;
        HealthBar.UpdateHealthBar(GetHealthPercent());
    }

    //Deal Damage to another unit
    public void DealDamage(Unit Target, string MoveName)
    {
        //Calculate Enemy Accuracy
        int AccuracyHitChance = UnityEngine.Random.Range(1, 101); // Random number between 1 and 100
        if (AccuracyHitChance <= CurrentAccuracy && GetPartyClass() == "Enemy")
        {
            Target.TakeDamage(CalculateDamage(Target, MoveName));
        } 
        else if(GetPartyClass() == "Enemy")
        {
            Debug.Log($"{Name} missed");
        }
        else
        {
            Target.TakeDamage(CalculateDamage(Target, MoveName));
        }

        if (Target.HealthBar != null)
        {
            Target.HealthBar.UpdateHealthBar(Target.GetHealthPercent());
        }
        else
        {
            Debug.LogWarning($"Target {Target.Name} has no HealthBar assigned.");
        }
    }
    
    //Calculates damage to deal to another unit based on attack and defense stats, etc.
    public int CalculateDamage(Unit Target, string MoveName)
    {
        //Calculate Dodge
        int DodgeHitChance = UnityEngine.Random.Range(1, 101); // Random number between 1 and 100
        if (DodgeHitChance <= Target.GetCurrentDodgeChance())
        {
            Debug.Log($"{Target.GetName()} dodged the attack!");
            return 0;
        }
        else
        {
            //Calculate Crit
            int CriticalHitChance = UnityEngine.Random.Range(1, 101); // Random number between 1 and 100
            int DamageAmount = CurrentAttack;

            if (CriticalHitChance <= CurrentCritChance)
            {
                DamageAmount = (int)(DamageAmount * 1.5f); // 50% more damage on critical hit
                Debug.Log($"{Name} landed a Critical Hit!");
            }

            switch (MoveName)
            {
                case "Slash":
                case "Shield Bash":
                case "War Cry":
                    return Mathf.Max(0, DamageAmount - Target.CurrentDefense);         //Basic damage calculation (if negative, return 0)
                case "Tackle":
                case "Bite":
                case "Stomp":
                    return Mathf.Max(0, DamageAmount - Target.CurrentDefense);         //Basic damage calculation (if negative, return 0)
                case "Fireball":
                case "Ice Spike":
                case "Lightning Bolt":
                    return Mathf.Max(0, DamageAmount - Target.CurrentDefense);         //Basic damage calculation (if negative, return 0)
                case "Backstab":
                case "Poison Dart":
                case "Vanish":
                    return Mathf.Max(0, DamageAmount - Target.CurrentDefense);         //Basic damage calculation (if negative, return 0)
                default:
                    return Mathf.Max(0, DamageAmount - Target.CurrentDefense);
            }
        }
    }

    // Applies damage to the unit, reducing current health
    public void TakeDamage(int Amount)
    {
        //Take Damage
        CurrentHealth -= Amount;
        //Clamp Health to not go below 0
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        if (HealthBar != null)
        {
            HealthBar.UpdateHealthBar(GetHealthPercent());
        }
        else
        {
            Debug.LogWarning($"Unit {Name} has no HealthBar assigned.");
        }
    }

    public int QuickTimeEventType(string MoveName)
    {
        switch (MoveName)
        {
            case "Slash":
            case "Fireball":
            case "Backstab":
                return 0;         //Type 0
            case "Shield Bash":
            case "Ice Spike":
            case "Poison Dart":
                return 1;         //Type 1
            case "War Cry":
            case "Lightning Bolt":
            case "Vanish":
                return 2;         //Type 2
            default:
                return 0;         //Type 0
        }
    }

    //Checks if the unit is still alive
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    //Checks if the unit is Dead
    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    // Restores mana up to the maximum limit
    public void RestoreMana(int Amount)
    {
        if(CurrentMana + Amount > MaxMana)
        {
            CurrentMana = MaxMana;
        } else
        {
            CurrentMana += Amount;
        }
    }

    // Uses mana for an action if enough mana is available
    public bool UseMana(string MoveName)
    {
        int Amount = 0;

        switch (MoveName)
        {
            case "Slash":
            case "Shield Bash":
            case "War Cry":
                Amount = 5;         //Mana cost
                break;
            case "Tackle":
            case "Bite":
            case "Stomp":
                Amount = 5;         //Mana cost
                break;
            case "Fireball":
            case "Ice Spike":
            case "Lightning Bolt":
                Amount = 5;         //Mana cost
                break;
            case "Backstab":
            case "Poison Dart":
            case "Vanish":
                Amount = 5;         //Mana cost
                break;
            default:
                Amount = 5;         //Mana cost
                break;
        }

        if (HasEnoughMana(Amount))
        {
            CurrentMana -= Amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Checks if the unit has enough mana to perform an action
    public bool HasEnoughMana(int Amount)
    {
        return CurrentMana >= Amount;
    }

    // Resets temporary stat changes and adds experience (e.g., after battle)
    public void ResetStats(int GivenExperience)
    {
        CurrentAttack = Attack;
        CurrentDefense = Defense;
        CurrentSpeed = Speed;
        CurrentCritChance = CritChance;
        CurrentDodgeChance = DodgeChance;
        CurrentAccuracy = Accuracy;

        Experience += GivenExperience;
        if(Experience >= MaxExperience)
        {
            Experience -= MaxExperience;
            LevelUp();
        }
    }
}
