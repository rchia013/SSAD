using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles the UI for the Instructions page in the Main Menu
/// </summary>
public class Instructions_Controller : MonoBehaviour
{
    //Instruction UI objects
    public GameObject controlsPicture;
    public GameObject gameplayPicture;

    /// <summary>
    /// Activates the Instructions UI for the controls.
    /// </summary>
    public void activateControlsPicture()
    {
        controlsPicture.SetActive(true);
        gameplayPicture.SetActive(false);

    }
    /// <summary>
    /// Activates the Instructions UI for the gameplay.
    /// </summary>
    public void activateGamePlayPicture()
    {
        controlsPicture.SetActive(false);
        gameplayPicture.SetActive(true);

    }


}
