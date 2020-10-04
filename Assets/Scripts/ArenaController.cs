using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public GameObject[] maps;
    public int activeMapIndex;


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