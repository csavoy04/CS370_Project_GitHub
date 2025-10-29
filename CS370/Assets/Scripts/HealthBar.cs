using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float healthPercent)
    {
        if (slider == null)
        {
            Debug.LogWarning($"HealthBar on '{gameObject.name}' has no Slider assigned.", this);
            return;
        }
        slider.value = healthPercent;
    }

}
