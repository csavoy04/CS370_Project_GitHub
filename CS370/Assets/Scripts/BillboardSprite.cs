using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main)
            transform.forward = Camera.main.transform.forward;
    }
}