using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// This class handles the colour of the Ball Character that the player chooses
/// </summary>
public class BallMarkerSet : MonoBehaviour
{
    // Different colours that the ball character can become
    public Material blue;
    public Material pink;
    public Material green;
    public Material yellow;
    public Material purple;
    public Material brown;

    // The parts of the ball character that the colour can be changed
    MeshRenderer rend1;
    MeshRenderer rend2;
    MeshRenderer rend3;
    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("setColor", RpcTarget.All);
    }

    /// <summary>
    /// Sets the colour of the ball character that the player has chosen.
    /// </summary>
    [PunRPC]
    private void setColor()
    {
        // Determines which part of the ball character will change to the different colours
        rend1 = gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<MeshRenderer>();
        rend2 = gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<MeshRenderer>();
        rend3 = gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.GetComponent<MeshRenderer>();
        Material[] materials1 = rend1.materials;
        Material[] materials2 = rend2.materials;
        Material[] materials3 = rend3.materials;
        Material chosen = null;

        //Determines the colour that the player has chosen.
        switch (gameObject.GetComponent<PlayerController>().colorIndex)
        {
            case 1:
                chosen = blue;
                break;

            case 2:
                chosen = pink;
                break;

            case 3:
                chosen = green;
                break;

            case 4:
                chosen = yellow;
                break;

            case 5:
                chosen = purple;
                break;

            case 6:
                chosen = brown;
                break;

        }

        // Sets the colours of the different parts of the ball character based on the players choice
        materials1[0] = chosen;
        materials2[0] = chosen;
        materials3[0] = chosen;

        rend1.materials = materials1;
        rend2.materials = materials2;
        rend3.materials = materials3;
    }
}
