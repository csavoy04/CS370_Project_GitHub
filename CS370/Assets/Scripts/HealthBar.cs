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
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            if (healthPercent > 0.2f)
            {
                fillImage.color = Color.yellow;
            }
            else if (healthPercent == 0f)
            {
                fillImage.color = Color.clear;
            }
            else
            {
                fillImage.color = Color.red;
            }
        }
    }

}
