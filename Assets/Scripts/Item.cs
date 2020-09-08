using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public bool isFull;
    public GameObject slot;

    void Start()
    {
        slot = GameSetUp.GS.slot;
    }

}
