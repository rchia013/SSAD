using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float speedIncrease = 2f;
    public float duration = 4f;
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hidePowerUp();

            StartCoroutine(Pickup(other));
        }

    }
    private IEnumerator Pickup(Collider other)
    {
        GetComponent<CapsuleCollider>().enabled = false;

        Movement stats = other.GetComponent<Movement>();
        stats.speed += speedIncrease;

        yield return new WaitForSeconds(duration);

        stats.speed -= speedIncrease;

        ResetPowerUp();

    }

    void hidePowerUp()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale *= 0;
    }

    void ResetPowerUp()
    {
        gameObject.SetActive(false);

        GetComponent<Collider>().enabled = true;
        transform.localScale = originalScale;
        
    }
}