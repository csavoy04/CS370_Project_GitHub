using UnityEngine;

public class AnimationSpawner : MonoBehaviour
{
    public GameObject bitePrefab;
    public Transform spawnPoint;

    public void SpawnBite(Unit Defender)
    {
        float x = 0;
        float y = 0;
        float z = 0;

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
            y = 1;
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
            y = 1;
        }

        Vector3 spawnPos = new Vector3(x, y, z);
        GameObject bite = Instantiate(bitePrefab, spawnPos, Quaternion.identity);
        bite.transform.forward = Camera.main.transform.forward;
        Destroy(bite, 1.167f);
    }
}