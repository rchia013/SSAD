using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Item inventory;
    public GameObject itemButton;

    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1" | other.gameObject.tag == "Player2"| other.gameObject.tag == "Player3"| other.gameObject.tag == "Player4")
        {
            int playerIndex = int.Parse(other.gameObject.tag.Substring(other.gameObject.tag.Length - 1));
            inventory = other.gameObject.GetComponent<Item>();

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

                Instantiate(itemButton, inventory.slot.transform, false);

                destroyPowerUp();
            }
            
        } 
    }

    void destroyPowerUp()
    {
        GetComponent<Collider>().enabled = false;
        transform.localScale *= 0;
        gameObject.SetActive(false);
        transform.localScale = originalScale;
        enabled = false;
    }
}
