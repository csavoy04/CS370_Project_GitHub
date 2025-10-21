using System;
using UnityEngine;

[System.Serializable]
public struct Unit
{
    //Stats
    public PartyClass PartyType;
    public string Name;
    public UnitClass ClassType;
    public int Level;
    public int Health;
    public int Mana;
    public int Attack;
    public int Defense;
    public int Speed;
    public string[] MoveSet;

    public enum PartyClass
    {
        Player,
        Enemy
    }

    public enum UnitClass
    {
        Warrior,
        Mage,
        Rogue,
        Slime,
        Goblin,
        Orc
    }

    public Unit (PartyClass GivenPartyType, string GivenName, UnitClass GivenClassType, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed, string[] GivenMoveSet)
    {
        PartyType = GivenPartyType;
        Name = GivenName;
        ClassType = GivenClassType;
        Level = GivenLevel;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
        Defense = GivenDefense;
        Speed = GivenSpeed;
        MoveSet = GivenMoveSet;
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

    public string GetPartyClass()
    {
        return PartyType.ToString();
    }
}
