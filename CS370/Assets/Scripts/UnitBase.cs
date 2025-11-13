using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    public int CurrentCritChance;       //Current Critical Chance stat
    public int CritChance;              //Modified Critical Chance stat during battle

    public int CurrentDodgeChance;      //Current Dodge Chance stat
    public int DodgeChance;             //Modified Dodge Chance stat during battle

    public int CurrentAccuracy;         //Current Accuracy stat
    public int Accuracy;                //Modified Accuracy stat during battle

    public StatusEffect[] StatusEffects;

    //Scaling Stats? (Physical/Magical)
    //Armor? (Phyiscal, Magical)
    //Resistances? (Fire, Ice, Lightning, Poison, etc.)

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

    [System.Serializable]
    public struct StatusEffect
    {
        public StatusEffectType Type;
        public int Duration;
        public int Stacks;
        public int Damage;
    }

    public enum StatusEffectType
    {
        //DOT
        Burning,
        //Poisoned
        Bleeding
        //Cursed

        //Stat Buffs
        //Haste
        //Regen
        //Barrier
        //Focused
        //Inspired
        //Fortified
        //Reflect
        //EvasionUp
        //Counter

        //Stat Debuff
        //Slowed
        //Weakened
        //Broken
        //Silenced
        //Blinded
        //Confused
        //Feared
        //Staggered
        //Exhausted

        //Controlling Effects
        //Stunned
        //Frozen
        //Slept
    }

    public Unit (PartyClass GivenPartyType, string GivenName, UnitClass GivenClassType, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed, int GivenCritChance, int GivenDodgeChance, int GivenAccuracy, string[] GivenMoveSet, StatusEffect[] GivenStatusEffects)
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

        StatusEffects = GivenStatusEffects;
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

    public int GetCritChance()
    {
        return CurrentCritChance;
    }

    public int GetBaseCritChance()
    {
        return CritChance;
    }

    public int GetDodgeChance()
    {
        return CurrentDodgeChance;
    }

    public int GetBaseDodgeChance()
    {
        return DodgeChance;
    }

    public int GetAccuracy()
    {
        return CurrentAccuracy;
    }

    public int GetBaseAccuracy()
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
            ApplyStatusEffects(Target, MoveName);
        } 
        else if(GetPartyClass() == "Enemy")
        {
            Debug.Log($"{Name} missed");
        }
        else
        {
            Target.TakeDamage(CalculateDamage(Target, MoveName));
            ApplyStatusEffects(Target, MoveName);
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
        if (DodgeHitChance <= Target.GetDodgeChance())
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

    public void ApplyStatusEffects(Unit Target, string MoveName)
    {
        StatusEffect TempStatusEffect = new StatusEffect();
        switch (MoveName)
        {
            case "Slash":
            case "Shield Bash":
            case "War Cry":
                break;
            case "Tackle":
            case "Bite":
            case "Stomp":
                TempStatusEffect.Type = StatusEffectType.Burning;
                TempStatusEffect.Duration = 2;
                TempStatusEffect.Stacks = 1;
                TempStatusEffect.Damage = Mathf.FloorToInt(Target.GetMaxHealth() / 50.0f);
                Target.GiveStatusEffect(TempStatusEffect);
                break;
            case "Fireball":
            case "Ice Spike":
            case "Lightning Bolt":
                break;
            case "Backstab":
            case "Poison Dart":
            case "Vanish":
                break;
            default:
                break;
        }
    }
    public void GiveStatusEffect(StatusEffect NewEffect)
    {
        //Make sure array exists
        if (StatusEffects == null)
        {
            StatusEffects = new StatusEffect[0];
        }

        bool TempFound = false;
        //Check if this effect already exists
        for (int i = 0; i < StatusEffects.Length; i++)
        {
            if (StatusEffects[i].Type == NewEffect.Type)
            {
                TempFound = true;
                switch (NewEffect.Type)
                {
                    case (StatusEffectType.Burning):
                        StatusEffects[i].Duration += NewEffect.Duration;
                        break;
                    case (StatusEffectType.Bleeding):
                        StatusEffects[i].Stacks += NewEffect.Stacks;
                        break;
                    default:
                        break;
                }
                Debug.Log($"{Name} is now affected by {StatusEffects[i].Type} for {StatusEffects[i].Duration} turns!");
            }
        }
        if (!TempFound)
        {
            // Add new status effect
            Array.Resize(ref StatusEffects, StatusEffects.Length + 1);
            StatusEffects[^1] = NewEffect;
            Debug.Log($"{Name} is now affected by {NewEffect.Type} for {NewEffect.Duration} turns!");
        }
    }

    public void DamageStatusEffects()
    {
        for (int i = 0; i < StatusEffects.Length; i++)
        {
            switch (StatusEffects[i].Type)
            {
                case StatusEffectType.Burning:
                    CurrentHealth -= StatusEffects[i].Damage;
                    Debug.Log(StatusEffects[i].Damage + " Burning Damage Taken");
                    break;
                default:
                    break;
            }

            if (StatusEffects[i].Duration > 1)
            {
                StatusEffects[i].Duration -= 1;
            } else
            {
                StatusEffect[] newArray = new StatusEffect[StatusEffects.Length - 1];

                for (int index = 0, j = 0; index < StatusEffects.Length; index++)
                {
                    if (index == i) continue;
                    newArray[j++] = StatusEffects[i];
                }

                StatusEffects = newArray;
            }

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }
    }

    public void UpdateStatusEffects()
    {
        //Update damage values
    }
    
}
