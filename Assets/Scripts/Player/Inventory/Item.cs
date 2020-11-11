using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the inventory of the player in the game
/// </summary>
public class Item : MonoBehaviour
{
    // Inventory parameters
    public bool isFull;
    public GameObject slot;

    void Start()
    {
        slot = GameSetUp.GS.slot;
    }

}
