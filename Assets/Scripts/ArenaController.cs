using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public GameObject[] maps;
    public int activeMapIndex;

    //public GameObject map1;
    //public GameObject map2;
    //public GameObject map3;
    //public GameObject map4;
    //public GameObject map5;


    // Start is called before the first frame update

    public void setUpMap(int mapIndex)
    {
        activeMapIndex = mapIndex;

        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(false);
        }

        maps[mapIndex].SetActive(true);
    }
}
