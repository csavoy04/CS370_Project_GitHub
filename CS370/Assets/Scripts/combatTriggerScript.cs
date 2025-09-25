using UnityEngine;
using UnityEngine.SceneManagement;

public class combatTriggerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //if EnemyID == 0
            SceneManager.LoadScene("CombatArea");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
