using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is assigned to the group of active blocks within each arena.
/// It holds different materials, which correspond to each player's tile colors, as well as the tile colors for the countdown before the tile drops.
/// Activated and Special Blocks can call the functions in this script to obtain the material which should be displayed, based on the scenario.
/// </summary>

public class TileColorController : MonoBehaviour
{
    // materials 1-4 store the Countdown Materials

    public Material material4;
    public Material material3;
    public Material material2;
    public Material material1;

    // activeMaterials 1-6 store Player Tile Materials

    public Material activeMaterial1;
    public Material activeMaterial2;
    public Material activeMaterial3;
    public Material activeMaterial4;
    public Material activeMaterial5;
    public Material activeMaterial6;


    /// <summary>
    /// returns material to be displayed by blocks based on time remaining (int num) for question countdown
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>

    public Material getCountdownMaterial(int num)
    {
        switch (num)
        {
            case 1:
                return material1;
            case 2:
                return material2;
            case 3:
                return material3;
            case 4:
                return material4;
        }

        return null;
    }

    /// <summary>
    /// returns material to be displayed by blocks based on the colorIndex (int colorIndex) of the player the block is assigned to
    /// </summary>
    /// <param name="colorIndex"></param>
    /// <returns></returns>

    public Material getTileMaterial(int colorIndex)
    {
        switch (colorIndex)
        {
            case 1:
                return activeMaterial1;
            case 2:
                return activeMaterial2;
            case 3:
                return activeMaterial3;
            case 4:
                return activeMaterial4;
            case 5:
                return activeMaterial5;
            case 6:
                return activeMaterial6;
        }

        return null;
    }
}
