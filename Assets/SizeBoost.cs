using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBoost : MonoBehaviour
{
    // Start is called before the first frame update
    public float multiplier = 1.4f;
    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    
    void Pickup(Collider player)
    {
        player.transform.localScale *= multiplier;
        Destroy(gameObject);

    }
}
