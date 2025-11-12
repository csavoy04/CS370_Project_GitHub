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
    public float speed = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 contact = collision.GetContact(0).point;
            Vector3 position = collision.transform.position;
            Vector3 newPos = position;

            /* In the newPosition.axis = statement, the ? denotes that if 
                  the the speed * Time.deltaTime is Null (meaning the player
                  collides with the cube from the opposite direction AKA -x/-y
                  then newPosition.x = the default value (or in this case -speed)
                  V Link with more info V
                  https://cxyda.github.io/UnitysEqualityAndNullPropagationOperators
             */
            if ((contact.x - position.x) > (contact.y - position.y))
            {
                newPos.x += (contact.x > position.x) ? speed * Time.deltaTime : -speed * Time.deltaTime;
            }
            else
            {
                newPos.y += (contact.y > position.y) ? speed * Time.deltaTime : -speed * Time.deltaTime;
            }
            collision.transform.position = newPos;
        }
    }
}