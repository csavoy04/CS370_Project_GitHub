using System;
using UnityEngine;

[System.Serializable]
public struct Character
{
    //Stats
    public string Name;
    public int Level;
    public int Health;
    public int Mana;
    public int Attack;
    public int Defense;
    public int Speed;
    public string[] MoveSet;

    public Character(string GivenName, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed)
    {
        Level = GivenLevel;
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
        Defense = GivenDefense;
        Speed = GivenSpeed;
        MoveSet = new string[] { "Slash", "Fireball", "Backstab" };
    }

    public void LevelUp()
    {
        Level += 1;
        Health += 10;
        Mana += 5;
        Attack += 2;
        Defense += 2;
        Speed += 1;
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
    
    
}
