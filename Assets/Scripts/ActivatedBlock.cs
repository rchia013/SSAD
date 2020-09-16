using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ActivatedBlock : MonoBehaviourPunCallbacks
{
    public Material material4;
    public Material material3;
    public Material material2;
    public Material material1;

    public Material activeMaterial1;
    public Material activeMaterial2;
    public Material activeMaterial3;
    public Material activeMaterial4;

    public Material originalMaterial;

    GameObject parentBlock;
    GameObject highlight;

    MeshRenderer rend;
    Rigidbody rb;

    public bool blockActivated = false;
    bool questionActivated = false;
    public int playerIndex;
    string playerTag;

    GameObject player;
    GameObject question;

    PhotonView PV;



    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
        parentBlock = transform.parent.gameObject;

        rend = parentBlock.GetComponent<MeshRenderer>();
        rb = parentBlock.GetComponent<Rigidbody>();

        highlight = parentBlock.transform.GetChild(1).gameObject;

        rb.isKinematic = true;

        Material[] materials = rend.materials;

        switch (playerIndex)
        {
            case 1:
                materials[0] = activeMaterial1;
                break;
            case 2:
                materials[0] = activeMaterial2;
                break;
            case 3:
                materials[0] = activeMaterial3;
                break;
            case 4:
                materials[0] = activeMaterial4;
                break;

        }
        rend.materials = materials;

        gameObject.tag = "Question";

        playerTag = "Player" + playerIndex;

        parentBlock.transform.Translate(Vector3.up * .15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag & !questionActivated & blockActivated)
        {
            player = other.gameObject;
            question = player.GetComponent<Movement>().question;

            // 1. Start Block

            // a. Move Parent

            parentBlock.transform.Translate(Vector3.down * 0.1f);
            questionActivated = true;

            // b. Activate Highlight

            highlight.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine("HighlightFadeIn");

            Physics.IgnoreCollision(player.GetComponent<CharacterController>(), highlight.GetComponent<CapsuleCollider>(), true);
            Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), highlight.GetComponent<CapsuleCollider>(), true);

            highlight.GetComponent<CapsuleCollider>().enabled = true;

            // 2. Restrict Player

            player.GetComponent<Movement>().moveable = false;

            // 4. Start Question

            StartCoroutine("Question");
        }
    }

    IEnumerator Question()
    {
        // Determines Question Time
        int counter = 5;

        question.GetComponent<DoQuestion>().answered = false;
        question.GetComponent<DoQuestion>().correct = false;
        question.GetComponent<DoQuestion>().pointsAwardable = true;
        question.GetComponent<DoQuestion>().playerTag = playerTag;

        question.SetActive(true);

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);

            counter--;

            var materials = rend.materials;

            // Case 1: 

            if (question.GetComponent<DoQuestion>().answered == true && question.GetComponent<DoQuestion>().correct == true)
            {
                player.GetComponent<Movement>().moveable = true;
                question.SetActive(false);

                StartCoroutine("HighlightFadeOut");
             
            }

            else if (question.GetComponent<DoQuestion>().answered == true && question.GetComponent<DoQuestion>().correct == false)
            {
                StartCoroutine("HighlightFadeOut");

                PV.RPC("dropBlock", RpcTarget.All);

                yield return new WaitForSeconds(1);

                Destroy(transform.parent.gameObject);
                break;
            }

                switch (counter)
            {
                case 4:
                    materials[0] = material4;
                    break;

                case 3:
                    materials[0] = material3;
                    break;

                case 2:
                    materials[0] = material2;
                    break;

                case 1:
                    materials[0] = material1;
                    break;

                case 0:

                    if (question.GetComponent<DoQuestion>().answered == false)
                    {
                        StartCoroutine("HighlightFadeOut");
                    }

                    PV.RPC("dropBlock", RpcTarget.All);

                    yield return new WaitForSeconds(1);

                    Destroy(transform.parent.gameObject);
                    break;
            }

            rend.materials = materials;
        }
    }

    IEnumerator HighlightFadeIn()
    {
        MeshRenderer highlightRend = highlight.GetComponent<MeshRenderer>();
        Color highlightColor = highlightRend.material.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newColor = new Color(highlightColor.r, highlightColor.g, highlightColor.b, Mathf.Lerp(0, highlightColor.a, t));

            highlightRend.material.SetColor("_Color", newColor);

            yield return null;
        }
    }

    IEnumerator HighlightFadeOut()
    {
        MeshRenderer highlightRend = highlight.GetComponent<MeshRenderer>();
        Color highlightColor = highlightRend.material.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newColor = new Color(highlightColor.r, highlightColor.g, highlightColor.b, Mathf.Lerp(highlightColor.a, 0, t));

            highlightRend.material.SetColor("_Color", newColor);

            yield return null;
        }
    }

    [PunRPC]
    void dropBlock()
    {
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), highlight.GetComponent<CapsuleCollider>(), false);
        Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), highlight.GetComponent<CapsuleCollider>(), false);

        highlight.GetComponent<CapsuleCollider>().enabled = false;

        rb.isKinematic = false;
    }



    private void OnDisable()
    {
        gameObject.tag = "Untagged";

        if (rb.isKinematic)
        {
            Material[] materials = rend.materials;
            materials[0] = originalMaterial;
            rend.materials = materials;

            parentBlock.transform.Translate(Vector3.down * .15f);
        }
    }
}
