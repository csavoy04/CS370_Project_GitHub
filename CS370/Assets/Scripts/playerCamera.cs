using UnityEngine;
using UnityEngine.UIElements;

public class BasicFollow : MonoBehaviour {

    public Transform player;
    public Vector3 locationOffset;
    bool isRunning;

    void start()
    {
        bool isRunning = GameObject.Find("Player").GetComponent<Movement>().isRunning;
    }

    // Update is called once per frame
    void Update () {
        if (isRunning)
        {
            transform.position = player.transform.position + locationOffset;
        }
        else
        {
            // This should make the camera lag behind the player while the player is running
            //transform.position = Vector3.MoveTowards(transform.position, player.position, 0.5f * Time.deltaTime);
        }
    }
}