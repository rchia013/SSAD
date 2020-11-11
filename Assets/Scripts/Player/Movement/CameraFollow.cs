using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the camera angle and direction that follows the player when the character moves
/// Ensures that the player is always in the middle of the screen
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // Parameters of the camera and player
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
        // Determines the location of the camera whenever the player moves
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 4, -7);
        }

        // Camera zooms out when the player press 'Shift' to give wider angle of viewing
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
    /// <summary>
    /// Sets the target character to follow.
    /// </summary>
    /// <param name="target">The target.</param>
    public void setTarget(GameObject target)
    {
        playerTransform = target.transform;
    }

    /// <summary>
    /// Compares the target character to other characters.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns></returns>
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
