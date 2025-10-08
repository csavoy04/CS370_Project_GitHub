using System;
using UnityEngine;

[System.Serializable]
public struct Character
{
    //Stats
    public string Name;
    public CharacterClass ClassType;
    public int Level;
    public int Health;
    public int Mana;
    public int Attack;
    public int Defense;
    public int Speed;
    public string[] MoveSet;

    public enum CharacterClass
    {
        Warrior,
        Mage,
        Rogue
    }

    public Character(string GivenName, CharacterClass GivenClassType,int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed)
    {
        ClassType = GivenClassType;
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

    public void ClearMoveSet()
    {
        for (int i = 0; i < MoveSet.Length; i++)
        {
            MoveSet[i] = null;
        }
    }

    public void PrintMoveSet()
    {
        Debug.Log($"MoveSet for {Name}: {string.Join(", ", MoveSet)}");
    }

    public void PrintStats()
    {
        Debug.Log($"Name: {Name}, ClassType: {ClassType} Level: {Level}, Health: {Health}, Mana: {Mana}, Attack: {Attack}, Defense: {Defense}, Speed: {Speed}");
    }
}
