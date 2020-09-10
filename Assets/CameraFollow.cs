using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public string playerTag;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 4, -7);
        }
    }

    public void setTarget(GameObject target)
    {
        playerTransform = target.transform;
        print("helleefoe");
    }

    public bool compareTarget(string target)
    {
        if (target == playerTag)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
