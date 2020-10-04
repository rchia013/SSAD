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

    public GameObject canvas;

    public GameObject ArenaCon;
    public int mapIndex;

    public Transform[] spawnPoints1;
    public Transform[] spawnPoints2;
    public Transform[] spawnPoints3;
    public Transform[] spawnPoints4;

    public int playerIndex;

    public GameObject uiObject;
    public TextMeshProUGUI countdown;
    

    public GameObject player;
    public GameObject slot;
    public GameObject playerCam;

    public TextMeshProUGUI[] pointsUIList;

    GameObject question;
    
    private void OnEnable()
    {
        if (GameSetUp.GS == null)
        {
            GameSetUp.GS = this;
        }
    }

    void Start()
    {
        mapIndex = MapController.mapIndex;

        ArenaCon.GetComponent<ArenaController>().setUpMap(mapIndex);

        playerIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;

        string curUserName = PhotonNetwork.LocalPlayer.NickName;
        int avatarSelection = AvatarController.playerList[curUserName];

        print("Avatar Selection:" + avatarSelection);



        Transform[] spawnPoints = null;

        switch (mapIndex)
        {
            case 0:
                spawnPoints = spawnPoints1;
                break;

            case 1:
                spawnPoints = spawnPoints2;
                break;

            case 2:
                spawnPoints = spawnPoints3;
                break;

            case 3:
                spawnPoints = spawnPoints4;
                break;
        }


        // Player

        string avatarPath = "";

        switch (avatarSelection / 10)
        {
            case 0:
                avatarPath = "Mummy";
                break;

            default:
                break;
        }

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", avatarPath), spawnPoints[playerIndex].transform.position, Quaternion.identity);

        // Camera

        playerCam = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Main Camera"), Vector3.zero, Quaternion.Euler(26.618f, 0f, 0f));
        playerCam.GetComponent<Camera>().enabled = true;
        playerCam.GetComponent<CameraFollow>().setTarget(player);

        // Panels

        question = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Question"), canvas.transform.position, Quaternion.identity);
        question.GetComponent<DoQuestion>().tag = "Q" + (playerIndex + 1);
        question.transform.SetParent(canvas.transform);
        question.SetActive(false);

        // points = pointsUIList[playerIndex];

        player.GetComponent<Movement>().playerName = curUserName;
        player.GetComponent<Movement>().question = question.gameObject;
        player.GetComponent<Movement>().colorIndex = avatarSelection % 10;

        player.gameObject.tag = "Player" + (playerIndex + 1);

        print(player.gameObject.tag);
    }
}
