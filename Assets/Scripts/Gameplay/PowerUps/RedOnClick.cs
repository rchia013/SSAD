using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the logic when the player uses a speed powerup
/// </summary>
public class RedOnClick : MonoBehaviour
{
    // Parameters for the powerup
    private Image image;
    private GameObject player;
    public float speedIncrease = 2f;
    public float duration = 4f;
    bool used = false;
    public int playerIndex;
    Vector3 originalScale;

    // Start is called before the first frame update
    private void Start()
    {
        string playerTag = "Player" + playerIndex;
        player = GameObject.FindGameObjectWithTag(playerTag);

        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame.
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Use();
        }
    }

    /// <summary>
    /// Function is called when the player uses the powerup.
    /// </summary>
    public void Use()
    {
        if (!used)
        {
            used = true;
            image.enabled = false;
            player.GetComponent<Item>().isFull = false;
            StartCoroutine("PowerUp");
        }

    }

    /// <summary>
    /// Increses the speed of the character when the player uses the speed powerup.
    /// </summary>
    private IEnumerator PowerUp()
    {
        PlayerController stats = player.GetComponent<PlayerController>();

        // Increase the speed of the player 
        stats.speed += speedIncrease;
        stats.boostSpeed(true);

        yield return new WaitForSeconds(duration);

        // Returns the speed of the player back to normal.
        stats.speed -= speedIncrease;
        stats.boostSpeed(false);

        // Destroy the powerup in the player's inventory
        Destroy(gameObject);
    }
}
