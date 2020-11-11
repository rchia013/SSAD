using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Displays the username of the character in the game
/// </summary>
public class Username : MonoBehaviour
{
    public Camera cameraToLookAt;
    void Start()
    {
        cameraToLookAt = GameSetUp.GS.playerCam.GetComponent<Camera>();
    }
    void Update()
    {
        // Handles the position of the username
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
