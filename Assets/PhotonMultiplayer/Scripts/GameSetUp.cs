using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.IO;

public class GameSetUp : MonoBehaviourPunCallbacks
{
    public static GameSetUp GS;

    public Transform[] spawnPoints;

    private int spawnIndex;

    public GameObject uiObject;
    public GameObject question;
    public TextMeshProUGUI countdown;

    public GameObject player;
    public GameObject slot;
    public GameObject playerCam;

    
    private void OnEnable()
    {
        if (GameSetUp.GS == null)
        {
            GameSetUp.GS = this;
        }
    }

    void Start()
    {
        spawnIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;

        // Player

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player111"), spawnPoints[spawnIndex].transform.position, Quaternion.identity);

        // Camera

        playerCam = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Main Camera"), Vector3.zero, Quaternion.Euler(26.618f, 0f, 0f));
        playerCam.GetComponent<Camera>().enabled = true;
        playerCam.GetComponent<CameraFollow>().setTarget(player);

        // Panels

        player.gameObject.tag = "Player" + (spawnIndex+1);
        Debug.Log(player.gameObject.tag);
    }
}
