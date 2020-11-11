using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles the logic when the player uses a jump powerup
/// </summary>
public class BlueOnClick : MonoBehaviour
{
    // Parameters for the powerup
    private Image image;
    private GameObject player;
    public float duration = 7f;
    bool used = false;
    public int playerIndex;

    // Start is called before the first frame update
    private void Start()
    {
        string playerTag = "Player" + playerIndex;
        player = GameObject.FindGameObjectWithTag(playerTag);

        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
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
        // Ensures that the player is not currently using any powerup
        if (!used)
        {
            used = true;
            image.enabled = false;
            player.GetComponent<Item>().isFull = false;
            StartCoroutine("PowerUp");
        }

    }

    /// <summary>
    /// Boosts the jump ability of the character when the player uses the jump powerup.
    /// </summary>
    private IEnumerator PowerUp()
    {
        // Get the current stats of the player
        PlayerController stats = player.GetComponent<PlayerController>();

        // Doubles the jump ability of the character
        stats.jumpHeight *= 2;
        stats.boostJump(true);

        yield return new WaitForSeconds(duration);

        // Returns the jump ability of the character back to normal.
        stats.jumpHeight /= 2;
        stats.boostJump(false);

        // Destroys the powerup in the player's inventory
        Destroy(gameObject);
    }
}
