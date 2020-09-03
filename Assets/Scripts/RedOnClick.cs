using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedOnClick : MonoBehaviour
{
    private Image image;
    private GameObject player;
    public float speedIncrease = 2f;
    public float duration = 4f;
    bool used = false;

    public int playerIndex;

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
        stats.speed += speedIncrease;

        yield return new WaitForSeconds(duration);

        stats.speed -= speedIncrease;

        Destroy(gameObject);
    }
}
