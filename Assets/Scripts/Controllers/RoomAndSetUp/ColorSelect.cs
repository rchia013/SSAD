using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    // Outline of selected color
    public Image outline;

    /// <summary>
    /// Sets the outline to active
    /// </summary>
    /// <param name="active"></param>
    public void outlineActive(bool active)
    {
        outline.gameObject.SetActive(active);
    }
}
