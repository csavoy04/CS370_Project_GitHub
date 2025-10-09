using UnityEngine;

[System.Serializable]
public struct Enemy
{
    //Stats
    public string Name;
    public EnemyType Type;
    public int Level;
    public int Health;
    public int Mana;
    public int Attack;
    public int Defense;
    public int Speed;

    public string[] EnemyMoveSet;

    public enum EnemyType
    {
        Slime,
        Goblin,
        Ork
    }


    public Enemy(string GivenName, EnemyType GivenType, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed)
    {
        Type = GivenType;
        Level = GivenLevel;
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
        Defense = GivenDefense;
        Speed = GivenSpeed;
        EnemyMoveSet = new string[] { "Tackle", "Bite", "Stomp" };
    }

    public void SetMoveSet
    {
        
    }

    public void PrintStats()
    {
        Debug.Log("Enemy Name: " + Name + " || Enemy Type: " + Type + " || Enemy Lvl: " + Level + " || Enemy HP: " + Health + " || Enemy MP: " + Mana + " || Enemy ATK: " + Attack + " || Enemy DEF: " + Defense + " || Enemy SPD: " + Speed);
    }
}

