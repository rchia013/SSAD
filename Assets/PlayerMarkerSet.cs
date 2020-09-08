using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerSet : MonoBehaviour
{

    public Material player4;
    public Material player3;
    public Material player2;
    public Material player1;

    MeshRenderer rend1;
    MeshRenderer rend2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.transform.GetChild(2));
        rend1 = gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        rend2 = gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();

        Material[] materials1 = rend1.materials;
        Material[] materials2 = rend2.materials;

        Material chosen = null;

        switch (gameObject.tag)
        {
            case "Player1":
                chosen = player1;
                break;

            case "Player2":
                chosen = player2;
                break;

            case "Player3":
                chosen = player3;
                break;

            case "Player4":
                chosen = player4;
                break;
        }

        materials1[0] = chosen;
        materials2[0] = chosen;

        rend1.materials = materials1;
        rend2.materials = materials2;

    }
}
