using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorController : MonoBehaviour
{
    // Countdown materials

    public Material material4;
    public Material material3;
    public Material material2;
    public Material material1;

    // Tile Materials

    public Material activeMaterial1;
    public Material activeMaterial2;
    public Material activeMaterial3;
    public Material activeMaterial4;
    public Material activeMaterial5;
    public Material activeMaterial6;

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
