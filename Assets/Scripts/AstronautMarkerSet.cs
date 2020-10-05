using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AstronautMarkerSet : MonoBehaviour
{
    public Material blue;
    public Material pink;
    public Material green;
    public Material yellow;
    public Material purple;
    public Material brown;

    SkinnedMeshRenderer rend1;

    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        PV.RPC("setColor", RpcTarget.All);
    }


    [PunRPC]
    private void setColor()
    {
        rend1 = gameObject.transform.Find("Player").gameObject.GetComponent<SkinnedMeshRenderer>();

        Material[] materials1 = rend1.materials;

        Material chosen = null;

        switch (gameObject.GetComponent<Movement>().colorIndex)
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

        materials1[0] = chosen;

        rend1.materials = materials1;
    }
}
