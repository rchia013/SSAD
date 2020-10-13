using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueOnClick : MonoBehaviour
{
    private Image image;
    private GameObject player;
    public float duration = 7f;
    bool used = false;

    public int playerIndex;

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
        PlayerController stats = player.GetComponent<PlayerController>();
        stats.jumpHeight *= 2;

        stats.boostJump(true);

        yield return new WaitForSeconds(duration);

        stats.jumpHeight /= 2;
        stats.boostJump(false);

        Destroy(gameObject);
    }
}
