using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private HealthSystem healthSystem;
    public void SetUp(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }
}
