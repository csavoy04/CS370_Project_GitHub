using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public HealthSystem healthSystem = new HealthSystem(100);
    public HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
