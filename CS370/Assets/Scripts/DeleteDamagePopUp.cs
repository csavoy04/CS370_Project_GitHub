using UnityEngine;

public class DeleteDamagePopUp : MonoBehaviour
{
    [SerializeField] private float delayBeforeDelete = 1.0f;
    void Start()
    {
        Destroy(gameObject, delayBeforeDelete);
    }
}
