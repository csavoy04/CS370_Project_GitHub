using UnityEngine;
using UnityEngine.AI;

public class SlimeMovement : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= chaseRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }

    //Occurs after Update() 
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
    
}
