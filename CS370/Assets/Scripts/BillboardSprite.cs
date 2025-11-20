using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public AnimationSpawner animationSpawner;

    Vector3 CurrentPos;
    Vector3 TargetDirection;

    void Start()
    {
        animationSpawner = Object.FindFirstObjectByType<AnimationSpawner>();

        CurrentPos = animationSpawner.SpawnPos;
        TargetDirection = (animationSpawner.TargetPos - CurrentPos).normalized;
    }

    void LateUpdate()
    {
        if (Camera.main)
            transform.forward = Camera.main.transform.forward;
    }

    private void Update()
    {
        if (animationSpawner.IsMovingAnimation == true)
        {
            CurrentPos += TargetDirection * Time.deltaTime * 10f;
            transform.position = CurrentPos;
        }
    }
}