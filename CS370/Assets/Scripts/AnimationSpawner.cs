using UnityEngine;

public class AnimationSpawner : MonoBehaviour
{
    public GameObject bitePrefab;
    public GameObject lightningPrefab;
    public Transform spawnPoint;

    public void SpawnAnimation(Unit Defender, string MoveName, float duration)
    {
        float x = 0;
        float z = 0;

        float offsety = 0;

        if(Defender.GetPartyClass() == "Player")
        {
            int allyCount = PartySystem.Instance.PlayerParty != null ? PartySystem.Instance.PlayerParty.Count : 0;
            for (int NoOfAllies = 0; NoOfAllies < allyCount; NoOfAllies++)
            {
                if (PartySystem.Instance.PlayerParty[NoOfAllies].GetPartyClass() != "Empty" && PartySystem.Instance.PlayerParty[NoOfAllies].GetCurrentHealth() > 0)
                {
                    if (Defender == PartySystem.Instance.PlayerParty[NoOfAllies])
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
                if (PartySystem.Instance.EnemyParty[NoOfEnemies] == Defender) 
                {
                    z = (NoOfEnemies - 1) * 3 - 0.7f;
                }
                
            }

            x = 4.85f;
        }

        switch (Defender.GetUnitClass())
        {
            case "Warrior":
            case "Mage":
            case "Rogue":
                offsety = 1;
                break;
            case "Slime":
                offsety = 1;
                break;
            default:
                offsety = 0;
                break;
        }

        Vector3 spawnPos = new Vector3(x, offsety, z);
        GameObject animation;
        switch (MoveName)
        {
            case "Bite":
                animation = Instantiate(bitePrefab, spawnPos, Quaternion.identity);
                Destroy(animation, duration);
                break;
            case "Lightning Bolt":
                animation = Instantiate(lightningPrefab, spawnPos, Quaternion.identity);
                Destroy(animation, duration);
                break;
            default:
                animation = Instantiate(bitePrefab, spawnPos, Quaternion.identity);
                Destroy(animation, duration);
                break;

        }
        animation.transform.forward = Camera.main.transform.forward;
    }
}