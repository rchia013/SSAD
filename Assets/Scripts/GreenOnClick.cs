using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GreenOnClick : MonoBehaviour
{
    private Image image;
    
    private GameObject player;
    private float _currentScale = InitScale;
    private const float TargetScale = 1.4f;
    private const float InitScale = .75f;
    private const int FramesCount = 100;
    private const float AnimationTimeSeconds = 1;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;
    public float duration = 4f;

    public int playerIndex;

    bool used = false;

    Vector3 originalScale;
    private void Start()
    {
        string playerTag = "Player" + playerIndex;
        player = GameObject.FindGameObjectWithTag(playerTag);
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Use();
        }
    }

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
    

    private IEnumerator PowerUp()
    {
        Movement stats = player.GetComponent<Movement>();

        while (_upScale)
        {
            stats.speed = 2;
            _currentScale += _dx;
            if (_currentScale > TargetScale)
            {
                _upScale = false;
                _currentScale = TargetScale;
            }
            player.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }


        yield return new WaitForSeconds(duration);


        while (!_upScale )
        {
            _currentScale -= _dx;
            if (_currentScale < InitScale)
            {
                _upScale = true;
                _currentScale = InitScale;
            }
            player.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }
        print("Shrink Finish");
        stats.speed += 2;

        Destroy(gameObject);
    }
}
