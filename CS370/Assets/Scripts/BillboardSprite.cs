using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public AnimationSpawner animationSpawner;

    Vector3 CurrentPos;
    Vector3 TargetDirection;

    private float elapsed = 0f;
    private float duration;

    public AnimationClip animationClip;

    void Start()
    {
        animationSpawner = Object.FindFirstObjectByType<AnimationSpawner>();

        CurrentPos = animationSpawner.SpawnPos;
        TargetDirection = (animationSpawner.TargetPos - CurrentPos).normalized;

        // Get the animation clip duration
        switch(animationSpawner.CurrentMoveName)
        {
            case "Fire Ball":
                duration = animationSpawner.FireBallPreFab.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
                break;
            case "Ice Spike":
                duration = animationSpawner.IceBulletPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
                break;
            default:
                duration = 1f; // Default duration if move name is not recognized
                break;
        }
    }

    void LateUpdate()
    {
        if (Camera.main)
            transform.forward = Camera.main.transform.forward;
    }

    private void Update()
    {
        if (animationSpawner.IsMovingAnimation)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            transform.position = Vector3.Lerp(animationSpawner.SpawnPos, animationSpawner.TargetPos, t);
        }
    }
}