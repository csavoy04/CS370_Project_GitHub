using UnityEngine;

/*  Script made by: Coleman
        Script that:
            - Allows blocks to slide upon contact with player
    Date: 11/10/2025
    Made in: C# VsCode
*/

public class SlidePuzzle : MonoBehaviour
{
    [Header("Variables")]
    public Vector3 direction;
    public float speed = 4f;

    void OnCollisionEnter(Collision collision)
    {
        /*------------------------------- GET COLLISION DIRECTION ---------------------*/
        var relativePosition = transform.InverseTransformPoint(collision.transform.position);
        if (relativePosition.x > 0)
        {
            direction = new Vector3(-50f, 0f, 0f);
        } else if (relativePosition.x < 0)
        {
            direction = new Vector3(50f, 0f, 0f);
        } else if (relativePosition.y > 0)
        {
           direction = new Vector3(0f, -50f, 0f);
        } else if (relativePosition.y < 0)
        {
           direction = new Vector3(0f, 50f, 0f);
        } else
        {
            direction = new Vector3(0f, 0f, 0f);
        }

        /*-------------------------------------- MOVE CUBE -----------------------------*/
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    } 
}