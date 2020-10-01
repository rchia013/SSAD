using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerSet : MonoBehaviour
{

    public Material blue;
    public Material pink;
    public Material green;
    public Material yellow;
    public Material purple;
    public Material orange;

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
                chosen = orange;
                break;

        }

        materials1[0] = chosen;
        materials2[0] = chosen;

        rend1.materials = materials1;
        rend2.materials = materials2;

    }
}
