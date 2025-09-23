using UnityEngine;
using UnityEngine.UIElements;

public class BasicFollow : MonoBehaviour {

    public Transform player;
    public Vector3 locationOffset;

    // Update is called once per frame
    void Update () {
        transform.position = player.transform.position + locationOffset;
    }
}