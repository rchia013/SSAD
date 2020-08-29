using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    public float threshold;

    void FixedUpdate()
    {
        if (transform.position.y < threshold)
            transform.position = new Vector3(3, 1.5f, 10);
    }
}