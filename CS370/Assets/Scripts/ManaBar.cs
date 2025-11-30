using UnityEngine;
using UnityEngine.UI;
public class ManaBar : MonoBehaviour
{
[SerializeField] private Slider slider;

    public void UpdateManaBar(float manaPercent)
    {
        if (slider == null)
        {
            Debug.LogWarning($"ManaBar on '{gameObject.name}' has no Slider assigned.", this);
            return;
        }
        slider.value = manaPercent;
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            if (manaPercent <= 0f)
            {
                fillImage.color = Color.clear;
            }
        }
    }
}
