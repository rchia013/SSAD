using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowBoost : MonoBehaviour
{
    public float multiplier = 1.4f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
        
    }

    // Update is called once per frame
    void Pickup(Collider other)
    {
        other.transform.localScale *= multiplier;
        Destroy(gameObject);

    }
}
