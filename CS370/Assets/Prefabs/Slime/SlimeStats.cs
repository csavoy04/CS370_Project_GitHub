using UnityEngine;

[CreateAssetMenu(fileName = "SlimeStats", menuName = "Scriptable Objects/SlimeStats")]
public class SlimeStats : ScriptableObject
{
    public int maxHealth = 350;
    public int currentHealth = 350;
}
