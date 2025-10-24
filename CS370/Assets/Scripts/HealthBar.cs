using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public HealthSystem healthSystem;
    
    public void SetUp(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += Unit_OnHealthChanged;
    }

    public void Unit_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }

}
