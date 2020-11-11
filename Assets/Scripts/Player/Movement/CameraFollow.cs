using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public string playerTag;

    private Camera cam;

    // Update is called once per frame

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 4, -7);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            cam.fieldOfView = 70;
            cam.transform.rotation = Quaternion.Euler(40f, 0f, 0f);
        }
        else
        {
            cam.fieldOfView = 40;
            cam.transform.rotation = Quaternion.Euler(26.618f, 0f, 0f);

        }
    }

    public void setTarget(GameObject target)
    {
        playerTransform = target.transform;
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
