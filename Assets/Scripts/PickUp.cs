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
        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Item>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.gameObject.GetComponent<Item>();
            if(inventory.isFull == false)
            {
                inventory.isFull = true;
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
    }
}
