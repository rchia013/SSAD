using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeBoost : MonoBehaviour
{

    private float _currentScale = InitScale;
    private const float TargetScale = 1.4f;
    private const float InitScale = 1f;
    private const int FramesCount = 50;
    private const float AnimationTimeSeconds = 1;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;

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
            StartCoroutine(Pickup(other));

        }

    }

    private IEnumerator Pickup(Collider other)
    {

        Movement stats = other.GetComponent<Movement>();

        while (_upScale)
        {
            stats.speed = 2;
            _currentScale += _dx;
            if (_currentScale > TargetScale)
            {
                _upScale = false;
                _currentScale = TargetScale;
            }
            other.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
            transform.localScale *= 0;
            GetComponent<Collider>().enabled = false;
        }


        yield return new WaitForSeconds(duration);


        while (!_upScale)
        {
            _currentScale -= _dx;
            if (_currentScale < InitScale)
            {
                _upScale = true;
                _currentScale = InitScale;
            }
            other.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }

        stats.speed += 2;

        destroyPowerUp();
        ResetPowerUp();

        

    }

    void destroyPowerUp()
    {
        GetComponent<Collider>().enabled = false;

        transform.localScale *= 0;

        gameObject.SetActive(false);
    }


    void ResetPowerUp()
    {
        GetComponent<Collider>().enabled = true;
        transform.localScale = originalScale;
    }
}
