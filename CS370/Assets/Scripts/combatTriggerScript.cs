using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class combatTriggerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "player")
        {
            //if EnemyID == 0
            SceneManager.LoadScene("CombatArea");

            Debug.Log("Going into Combat Area");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
