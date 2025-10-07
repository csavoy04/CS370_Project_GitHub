using UnityEngine;

[System.Serializable]
public struct Enemy
{
    //Stats
    public string Name;
    public int Level;
    public int Health;
    public int Mana;
    public int Attack;
    public int Defense;
    public int Speed;


    public Enemy(string GivenName, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack, int GivenDefense, int GivenSpeed)
    {
        Level = GivenLevel;
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
        Defense = GivenDefense;
        Speed = GivenSpeed;
    }

}

