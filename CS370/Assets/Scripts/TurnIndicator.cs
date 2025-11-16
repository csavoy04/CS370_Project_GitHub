using System;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{

    public GameObject TurnIndicatorPrefab;
    public CombatHandler CombatHandler;
    public bool Spawned;

    Unit CurrentUnit;

    float x, z, offsety;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawned = false;

        x = 0f;
        z = 0f;

        offsety = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentUnit = CombatHandler.CurrentUnit;

        if (CurrentUnit.GetPartyClass() != "Empty" && CurrentUnit.GetCurrentHealth() > 0)
        {
            if (Spawned == false && CombatHandler.CurrentUnit != null)
            {
                Spawned = true;

                if (CurrentUnit.GetPartyClass() == "Player")
                {
                    int allyCount = PartySystem.Instance.PlayerParty != null ? PartySystem.Instance.PlayerParty.Count : 0;
                    for (int NoOfAllies = 0; NoOfAllies < allyCount; NoOfAllies++)
                    {
                        if (PartySystem.Instance.PlayerParty[NoOfAllies].GetPartyClass() != "Empty" && PartySystem.Instance.PlayerParty[NoOfAllies].GetCurrentHealth() > 0)
                        {
                            if (CurrentUnit == PartySystem.Instance.PlayerParty[NoOfAllies])
                            {
                                z = (NoOfAllies - 1) * 3 - 0.7f;
                            }
                        }
                    }

                    x = -4.85f;
                }
                else
                {
                    int enemyCount = PartySystem.Instance.EnemyParty != null ? PartySystem.Instance.EnemyParty.Count : 0;
                    for (int NoOfEnemies = 0; NoOfEnemies < enemyCount; NoOfEnemies++)
                    {
                        if (PartySystem.Instance.EnemyParty[NoOfEnemies] == CurrentUnit)
                        {
                            z = (NoOfEnemies - 1) * 3 - 0.7f;
                        }

                    }

                    x = 4.85f;
                }

                switch (CurrentUnit.GetUnitClass())
                {
                    case "Warrior":
                    case "Mage":
                    case "Rogue":
                        offsety = 2;
                        break;
                    case "Slime":
                        offsety = 1.7f;
                        break;
                    default:
                        offsety = 0;
                        break;
                }


                Vector3 spawnPos = new Vector3(x, offsety, z);
                GameObject turnIndicator = Instantiate(TurnIndicatorPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
