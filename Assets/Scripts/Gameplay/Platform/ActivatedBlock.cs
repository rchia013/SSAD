using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ActivatedBlock : MonoBehaviourPunCallbacks
{
    public Material originalMaterial;

    TileColorController TCC;

    GameObject parentBlock;
    GameObject highlight;

    MeshRenderer rend;
    Rigidbody rb;

    public bool blockActivated = false;
    bool questionActivated = false;
    public int playerIndex;
    public int colorIndex;
    string playerTag;

    GameObject player;
    GameObject question;

    PhotonView PV;



    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
        parentBlock = transform.parent.gameObject;
        TCC = parentBlock.transform.parent.gameObject.GetComponent<TileColorController>();

        rend = parentBlock.GetComponent<MeshRenderer>();
        rb = parentBlock.GetComponent<Rigidbody>();

        highlight = parentBlock.transform.GetChild(1).gameObject;

        rb.isKinematic = true;

        Material[] materials = rend.materials;

        materials[0] = TCC.getTileMaterial(colorIndex);

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
            question = player.GetComponent<PlayerController>().question;

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

            player.GetComponent<PlayerController>().moveable = false;

            // 4. Start Question

            StartCoroutine("Question");
        }
    }

    IEnumerator Question()
    {
        // Determines Question Time

        question.GetComponent<DoQuestion>().answered = false;
        question.GetComponent<DoQuestion>().correct = false;
        question.GetComponent<DoQuestion>().pointsAwardable = true;
        question.GetComponent<DoQuestion>().playerTag = playerTag;

        question.SetActive(true);

        int counter = Mathf.RoundToInt(GameObject.FindWithTag("GameController").GetComponent<QuestionManager>().getTimeLimit());
        
        bool moveableChanged = false;

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);

            counter--;

            var materials = rend.materials;

            // Case 1: 

            if (question.GetComponent<DoQuestion>().answered == true && question.GetComponent<DoQuestion>().correct == true)
            {
                if (!moveableChanged)
                {
                    player.GetComponent<PlayerController>().moveable = true;
                    moveableChanged = true;
                }

                question.SetActive(false);

                StartCoroutine("HighlightFadeOut");
             
            }

            else if (question.GetComponent<DoQuestion>().answered == true && question.GetComponent<DoQuestion>().correct == false)
            {
                StartCoroutine("HighlightFadeOut");

                PV.RPC("dropBlock", RpcTarget.All);

                yield return new WaitForSeconds(3);

                Destroy(transform.parent.gameObject);

                if (!player.GetComponent<PlayerController>().respawning)
                {
                    player.GetComponent<PlayerController>().moveable = true;
                }
            }

            if (counter > 4)
            {
                //
            }
            else if (counter > 0)
            {
                materials[0] = TCC.getCountdownMaterial(counter);
                rend.materials = materials;
            }
            else
            {
                if (question.GetComponent<DoQuestion>().answered == false)
                {
                    StartCoroutine("HighlightFadeOut");
                }

                PV.RPC("dropBlock", RpcTarget.All);

                yield return new WaitForSeconds(3);

                Destroy(transform.parent.gameObject);
                
                if (!player.GetComponent<PlayerController>().respawning)
                {
                    player.GetComponent<PlayerController>().moveable = true;
                }
                
                break;
            }
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

        parentBlock.GetComponent<BoxCollider>().enabled = false;
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
