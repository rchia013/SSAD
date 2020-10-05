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
    private Collider coll;

    private float _currentScale;
    private float TargetScale;
    private float InitScale;
    private const int FramesCount = 100;
    private const float AnimationTimeSeconds = 1;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx;
    private bool _upScale = true;
    public float duration = 4f;

    public int playerIndex;

    bool used = false;

    Vector3 originalScale;
    private void Start()
    {
        string playerTag = "Player" + playerIndex;
        player = GameObject.FindGameObjectWithTag(playerTag);
        coll = player.GetComponent<BoxCollider>();
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
        PlayerController stats = player.GetComponent<PlayerController>();

        stats.boostSize(true);
        InitScale = player.transform.localScale.x;
        TargetScale = InitScale * 2;
        _currentScale = InitScale;

        _dx = (TargetScale - InitScale) / FramesCount;
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
            coll.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }

        stats.boostSize(false);


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
            coll.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }
        stats.speed += 2;

        Destroy(gameObject);
    }
}
