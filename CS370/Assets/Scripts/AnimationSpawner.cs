using UnityEngine;
using System.Collections;

public class AnimationSpawner : MonoBehaviour
{

    public AudioSource audioSource;

    public GameObject bitePrefab;
    public GameObject lightningPrefab;
    public GameObject FireBallPreFab;
    public GameObject IceBulletPrefab;
    public GameObject tacklePrefab;
    public GameObject stompPrefab;

    public Transform spawnPoint;

    float DefenderX = 0;
    float DefenderY = 0;
    float DefenderZ = 0;

    float AttackerX = 0;
    float AttackerY = 0;
    float AttackerZ = 0;

    public Vector3 SpawnPos;
    public Vector3 TargetPos;

    public bool IsMovingAnimation = false;

    public string CurrentMoveName;

    public void SpawnAnimation(Unit Attacker, Unit Defender, string MoveName, float duration)
    {
        CurrentMoveName = MoveName;
        GameObject animation;

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

        switch (MoveName)
        {
            case "Fireball":
            case "Ice Spike":
                SpawnPos = new Vector3(AttackerX, AttackerY, AttackerZ);
                TargetPos = new Vector3(DefenderX, DefenderY, DefenderZ);
                IsMovingAnimation = true;
                break;
            default:
                SpawnPos = new Vector3(DefenderX, DefenderY, DefenderZ);
                break;
        }

        switch (MoveName)
        {
            case "Bite":
                animation = Instantiate(bitePrefab, SpawnPos, Quaternion.identity);
                break;
            case "Lightning Bolt":
                animation = Instantiate(lightningPrefab, SpawnPos, Quaternion.identity);
                break;
            case "Fireball":
                animation = Instantiate(FireBallPreFab, SpawnPos, Quaternion.identity);
                break;
            case "Ice Spike":
                animation = Instantiate(IceBulletPrefab, SpawnPos, Quaternion.identity);
                break;
            case "Tackle":
                animation = Instantiate(tacklePrefab, SpawnPos, Quaternion.identity);
                break;
            case "Stomp":
                animation = Instantiate(stompPrefab, SpawnPos, Quaternion.identity);
                break;
            default:
                animation = Instantiate(bitePrefab, SpawnPos, Quaternion.identity);
                break;

        }
        StartCoroutine(TimerCoroutine(duration));
        Destroy(animation, duration);
        animation.transform.forward = Camera.main.transform.forward;
    }

    IEnumerator TimerCoroutine(float Seconds)
    {

        //Start Timer
        yield return new WaitForSeconds(Seconds);

        IsMovingAnimation = false;
        audioSource.Play();

    }
}