using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpecialBlock : MonoBehaviourPunCallbacks
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
    public string playerTag;
    GameObject question;

    //Powerup:

    public GameObject powerup1;
    public GameObject powerup2;

    public int choice = -1;
    private GameObject chosenPower = null;

    
    PhotonView PV;

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
        gameObject.tag = "SpecialQuestion";

        // Find parent & components

        parentBlock = transform.parent.gameObject;

        rend = parentBlock.GetComponent<MeshRenderer>();
        rb = parentBlock.GetComponent<Rigidbody>();

        highlight = parentBlock.transform.GetChild(1).gameObject;

        // Parent settings

        rb.isKinematic = true;

        Material[] materials = rend.materials;
        materials[0] = activeMaterial;
        rend.materials = materials;

        parentBlock.transform.Translate(Vector3.up * .15f);

        // Add powerup

        switch (choice)
        {
            case 0:
                chosenPower = powerup1;
                break;

            case 1:
                chosenPower = powerup2;
                break;

            default:
                break;

        }

        setUpPowerUp(chosenPower);

    }

    void setUpPowerUp(GameObject powerup)
    {
        // Set parent
        powerup.transform.parent = parentBlock.transform;

        // Adjust position
        powerup.transform.position = parentBlock.transform.position;
        powerup.transform.Translate(new Vector3(0, 1.5f, 0));

        powerup.GetComponent<Rigidbody>().isKinematic = true;

        // Activate
        powerup.SetActive(true);
        powerup.GetComponent<PickUp>().enabled = true;
    }

    void givePowerUp(GameObject powerup)
    {
        powerup.GetComponent<Rigidbody>().isKinematic = false;
        powerup.GetComponent<CapsuleCollider>().enabled = true;

        powerup.transform.parent = null;
    }

    void removePowerUp(GameObject powerup)
    {
        powerup.SetActive(false);

        powerup.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player1" | other.gameObject.tag == "Player2"| other.gameObject.tag == "Player3"| other.gameObject.tag == "Player4") & !questionActivated & blockActivated)
        {
            player = other.gameObject;
            playerTag = player.tag;
            question = player.GetComponent<Movement>().question;

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
            Physics.IgnoreCollision(chosenPower.GetComponent<CapsuleCollider>(), highlight.GetComponent<CapsuleCollider>(), true);



            highlight.GetComponent<CapsuleCollider>().enabled = true;

            // 4. Start Question

            StartCoroutine("Question");
        }
    }

    IEnumerator Question()
    {

        question.GetComponent<DoQuestion>().answered = false;
        question.GetComponent<DoQuestion>().correct = false;
        question.GetComponent<DoQuestion>().pointsAwardable = false;
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
                    player.GetComponent<Movement>().moveable = true;
                    moveableChanged = true;
                }

                question.SetActive(false);

                givePowerUp(chosenPower);

                StartCoroutine("HighlightFadeOut");
            }

            else if (question.GetComponent<DoQuestion>().answered == true && question.GetComponent<DoQuestion>().correct == false)
            {
                StartCoroutine("HighlightFadeOut");

                PV.RPC("dropSpecBlock", RpcTarget.All);
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

                    if (question.GetComponent<DoQuestion>().answered == false)
                    {
                        removePowerUp(chosenPower);
                        StartCoroutine("HighlightFadeOut");
                    }

                    PV.RPC("dropSpecBlock", RpcTarget.All);
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

    [PunRPC]
    void dropSpecBlock()
    {
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), highlight.GetComponent<CapsuleCollider>(), false);
        Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), highlight.GetComponent<CapsuleCollider>(), false);

        highlight.GetComponent<CapsuleCollider>().enabled = false;

        rb.isKinematic = false;

        removePowerUp(chosenPower);
    }
}
