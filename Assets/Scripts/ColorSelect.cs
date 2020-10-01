using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    public Image outline;

    public void outlineActive(bool active)
    {
        outline.gameObject.SetActive(active);
    }
}
