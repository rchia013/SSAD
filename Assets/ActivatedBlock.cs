using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActivatedBlock : MonoBehaviour
{
    public Material material4;
    public Material material3;
    public Material material2;
    public Material material1;

    public Material activeMaterial;
    public Material originalMaterial;

    GameObject parentBlock;
    GameObject highlight;

    MeshRenderer rend;
    Rigidbody rb;

    public bool blockActivated = false;
    bool questionActivated = false;

    GameObject player;
    public GameObject question;



    private void OnEnable()
    {
        parentBlock = transform.parent.gameObject;

        rend = parentBlock.GetComponent<MeshRenderer>();
        rb = parentBlock.GetComponent<Rigidbody>();

        highlight = parentBlock.transform.GetChild(1).gameObject;

        //Physics.IgnoreCollision(parentBlock.GetComponent<BoxCollider>(), highlight.GetComponent<CapsuleCollider>());

        rb.isKinematic = true;

        Material[] materials = rend.materials;
        materials[0] = activeMaterial;
        rend.materials = materials;

        gameObject.tag = "Question";

        parentBlock.transform.Translate(Vector3.up * .15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & !questionActivated & blockActivated)
        {
            print("Touch & Give Q");

            player = other.gameObject;

            // 1. Move Block

            parentBlock.transform.Translate(Vector3.down * 0.1f);
            questionActivated = true;


            // 2. Restrict Player

            player.GetComponent<Movement>().moveable = false;



            // 3. Activate Highlight

            highlight.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine("HighlightFadeIn");

            Physics.IgnoreCollision(player.GetComponent<CharacterController>(), highlight.GetComponent<CapsuleCollider>(), true);
            Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), highlight.GetComponent<CapsuleCollider>(), true);
            

            highlight.GetComponent<CapsuleCollider>().enabled = true;

            // 4. Start Question

            StartCoroutine("Question");
        }
    }

    IEnumerator Question()
    {
        // Determines Question Time
        int counter = 5;

        question.GetComponent<DoQuestion>().correct = null;
        question.GetComponent<DoQuestion>().pointsAwardable = true;
        question.SetActive(true);

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);

            counter--;

            var materials = rend.materials;

            // Case 1: 

            if (question.GetComponent<DoQuestion>().correct == true)
            {
                player.GetComponent<Movement>().moveable = true;
                question.SetActive(false);

                StartCoroutine("HighlightFadeOut");
             
            }

            else if (question.GetComponent<DoQuestion>().correct == false)
            {
                StartCoroutine("HighlightFadeOut");

                dropBlock();

                yield return new WaitForSeconds(1);

                print("Gone");

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

                    if (question.GetComponent<DoQuestion>().correct == null)
                    {
                        StartCoroutine("HighlightFadeOut");
                    }

                    dropBlock();
                    yield return new WaitForSeconds(1);

                    print("Gone");

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
