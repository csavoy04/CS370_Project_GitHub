using UnityEngine;
using System.Collections.Generic;


    public struct Enemy
{
    //Stats
    public string Name { get; set; }
    public int Level { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }

    public Enemy(string GivenName, int GivenLevel, int GivenHealth, int GivenMana, int GivenAttack)
    {
        Level = GivenLevel;
        Name = GivenName;
        Health = GivenHealth;
        Mana = GivenMana;
        Attack = GivenAttack;
    }
}

