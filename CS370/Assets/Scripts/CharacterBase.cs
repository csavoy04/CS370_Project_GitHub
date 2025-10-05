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

    public Character(string GivenName, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack)
    {
        Level = GivenLevel;
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
    }
}
