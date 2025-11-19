using UnityEngine;

public class DeleteDamagePopUp : MonoBehaviour
{
    [SerializeField] public static float delayBeforeDelete = 0.5f;
    void Start()
    {
        Destroy(gameObject, delayBeforeDelete);
    }
}
