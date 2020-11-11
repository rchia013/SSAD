using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// This class handles the logic when the player collides with the powerup.
/// </summary>
public class PickUp : MonoBehaviour
{
    // Parameters for the powerup
    private Item inventory;
    public GameObject itemButton;

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Enables the collider of the character.
    /// </summary>
    private void OnEnable()
    {
        GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        // Check if the tag of the colliding entity is a player
        if (other.gameObject.tag == "Player1" | other.gameObject.tag == "Player2"| other.gameObject.tag == "Player3"| other.gameObject.tag == "Player4")
        {
            int playerIndex = int.Parse(other.gameObject.tag.Substring(other.gameObject.tag.Length - 1));
            inventory = other.gameObject.GetComponent<Item>();

            // If the inventory is empty, allow the player to pickup the powerup
            if(inventory.isFull == false)
            {
                inventory.isFull = true;

                try
                {
                    itemButton.GetComponent<GreenOnClick>().playerIndex = playerIndex;
                }
                catch { }
                try { 
                    itemButton.GetComponent<RedOnClick>().playerIndex = playerIndex;
                }
                catch { }
                try
                {
                    itemButton.GetComponent<BlueOnClick>().playerIndex = playerIndex;
                }
                catch { }

                Instantiate(itemButton, inventory.slot.transform, false);

                destroyPowerUp();
            }
            
        } 
    }

    // Destroys the powerup once it has been picked up by the player
    void destroyPowerUp()
    {
        GetComponent<Collider>().enabled = false;
        transform.localScale *= 0;
        gameObject.SetActive(false);
        transform.localScale = originalScale;
        enabled = false;
    }
}
