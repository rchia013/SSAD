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

    
    private void OnEnable()
    {
        if (GameSetUp.GS == null)
        {
            GameSetUp.GS = this;
        }
    }

    void Start()
    {
        Debug.Log("HERRE");
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        spawnIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;
        Debug.Log(spawnIndex);
        Debug.Log(spawnPoints[spawnIndex].transform.position);
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player111"), spawnPoints[spawnIndex].transform.position, Quaternion.identity);


        player.gameObject.tag = "Player" + (spawnIndex+1);
        Debug.Log(player.gameObject.tag);
    }
}
