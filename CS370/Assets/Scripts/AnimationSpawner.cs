using UnityEngine;

public class AnimationSpawner : MonoBehaviour
{
    public GameObject bitePrefab;
    public GameObject lightningPrefab;
    public GameObject FireBallPreFab;

    public GameObject iceSpikePrefab;

    public GameObject tacklePrefab;

    public GameObject stompPrefab;

    public Transform spawnPoint;

    public void SpawnAnimation(Unit Attacker, Unit Defender, string MoveName, float duration)
    {
        float DefenderX = 0;
        float DefenderY = 0;
        float DefenderZ = 0;

        float AttackerX = 0;
        float AttackerY = 0;
        float AttackerZ = 0;

        if (Defender.GetPartyClass() == "Player")
        {
            int allyCount = PartySystem.Instance.PlayerParty != null ? PartySystem.Instance.PlayerParty.Count : 0;
            for (int NoOfAllies = 0; NoOfAllies < allyCount; NoOfAllies++)
            {
                if (PartySystem.Instance.PlayerParty[NoOfAllies].GetPartyClass() != "Empty" && PartySystem.Instance.PlayerParty[NoOfAllies].GetCurrentHealth() > 0)
                {
                    if (Defender == PartySystem.Instance.PlayerParty[NoOfAllies])
                    {
                        DefenderZ = (NoOfAllies - 1) * 3 - 0.7f;
                    }
                }
            }

            DefenderX = -4.85f;
        } 
        else
        {
            int enemyCount = PartySystem.Instance.EnemyParty != null ? PartySystem.Instance.EnemyParty.Count : 0;
            for (int NoOfEnemies = 0; NoOfEnemies < enemyCount; NoOfEnemies++)
            {
                if (PartySystem.Instance.EnemyParty[NoOfEnemies] == Defender) 
                {
                    DefenderZ = (NoOfEnemies - 1) * 3 - 0.7f;
                }
                
            }

            DefenderX = 4.85f;
        }

        if (Attacker.GetPartyClass() == "Player")
        {
            int allyCount = PartySystem.Instance.PlayerParty != null ? PartySystem.Instance.PlayerParty.Count : 0;
            for (int NoOfAllies = 0; NoOfAllies < allyCount; NoOfAllies++)
            {
                if (PartySystem.Instance.PlayerParty[NoOfAllies].GetPartyClass() != "Empty" && PartySystem.Instance.PlayerParty[NoOfAllies].GetCurrentHealth() > 0)
                {
                    if (Attacker == PartySystem.Instance.PlayerParty[NoOfAllies])
                    {
                        AttackerZ = (NoOfAllies - 1) * 3 - 0.7f;
                    }
                }
            }

            AttackerX = -4.85f;
        }
        else
        {
            int enemyCount = PartySystem.Instance.EnemyParty != null ? PartySystem.Instance.EnemyParty.Count : 0;
            for (int NoOfEnemies = 0; NoOfEnemies < enemyCount; NoOfEnemies++)
            {
                if (PartySystem.Instance.EnemyParty[NoOfEnemies] == Defender)
                {
                    AttackerZ = (NoOfEnemies - 1) * 3 - 0.7f;
                }

            }

            AttackerX = 4.85f;
        }

        switch (Defender.GetUnitClass())
        {
            case "Warrior":
            case "Mage":
            case "Rogue":
                DefenderY = 1;
                break;
            case "Slime":
                DefenderY = 1;
                break;
            default:
                DefenderY = 0;
                break;
        }

        switch (Attacker.GetUnitClass())
        {
            case "Warrior":
            case "Mage":
            case "Rogue":
                AttackerY = 1;
                break;
            case "Slime":
                AttackerY = 1;
                break;
            default:
                AttackerY = 0;
                break;
        }

        Vector3 spawnPos;
        switch (MoveName)
        {
            case "Fireball":
            case "Ice Spike":
                spawnPos = new Vector3(AttackerX, AttackerY, AttackerZ);
                break;
            default:
                spawnPos = new Vector3(DefenderX, DefenderY, DefenderZ);
                break;
        }
        GameObject animation;
        switch (MoveName)
        {
            case "Bite":
                animation = Instantiate(bitePrefab, spawnPos, Quaternion.identity);
                break;
            case "Lightning Bolt":
                animation = Instantiate(lightningPrefab, spawnPos, Quaternion.identity);
                break;
            case "Fireball":
                animation = Instantiate(FireBallPreFab, spawnPos, Quaternion.identity);
                break;
            case "Ice Spike":
                animation = Instantiate(iceSpikePrefab, spawnPos, Quaternion.identity);
                break;
            case "Tackle":
                animation = Instantiate(tacklePrefab, spawnPos, Quaternion.identity);
                break;
            case "Stomp":
                animation = Instantiate(stompPrefab, spawnPos, Quaternion.identity);
                break;
            default:
                animation = Instantiate(bitePrefab, spawnPos, Quaternion.identity);
                break;

        }
        Destroy(animation, duration);
        animation.transform.forward = Camera.main.transform.forward;
    }
}