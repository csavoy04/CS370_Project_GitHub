using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class GameHandler : MonoBehaviour
{
    public HealthSystem healthSystem = new HealthSystem(100);
    public HealthBar healthBar;

    public int Money;
    public enum CombatAreaName { NA, SlimeField }

    public CombatAreaName CurrentCombatArea;

    public static GameHandler Instance;

    //Prevents duplicates and keeps between scenes
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // kill duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Money = 0;
        CurrentCombatArea = CombatAreaName.NA;

    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            healthSystem.GetHealth();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            healthSystem.TakeDamage(10);
            healthSystem.GetHealth();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            healthSystem.Heal(10);
            healthSystem.GetHealth();
        }
        */
    }
}
