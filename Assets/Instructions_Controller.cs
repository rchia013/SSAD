using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions_Controller : MonoBehaviour
{
    public GameObject controlsPicture;
    public GameObject gameplayPicture;

    public void activateControlsPicture()
    {
        controlsPicture.SetActive(true);
        gameplayPicture.SetActive(false);

    }

    public void activateGamePlayPicture()
    {
        controlsPicture.SetActive(false);
        gameplayPicture.SetActive(true);

    }


}
