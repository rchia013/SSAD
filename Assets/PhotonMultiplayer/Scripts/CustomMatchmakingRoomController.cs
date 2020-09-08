using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq.Expressions;
using System.Collections.Generic;

public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks {

    [SerializeField]
    private int multiPlayerSceneIndex;

    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject startButton;

    private PhotonView PV;

    [SerializeField]
    private Transform playersContainer;
    [SerializeField]
    private GameObject playerListingPrefab;

    [SerializeField]
    private Text roomNameDisplay;


    //[SerializeField]
    //private Dictionary<Vector3, bool> playerSpots = new Dictionary<Vector3, bool>();
    private Dictionary<Vector3, bool> playerSpots = new Dictionary<Vector3, bool>
    {
        [Vector3.zero] = false,
        [new Vector3(5, 5, 5)] = false,
        [new Vector3(-5, -5, -5)] = false,
        [new Vector3(10, 2, 6)] = false
    };

    /*
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    */

    void ClearPlayerListings()
    {
        for (int i=  playersContainer.childCount-1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }
    void ListPlayers()
    {
        int playerListLength = PhotonNetwork.PlayerList.Length;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("HEEY");
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        ClearPlayerListings();
        ListPlayers();
        CreatePlayer();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
    }
    
    private void CreatePlayer()
    {
        Vector3 spawnPoint = Vector3.zero;
        foreach (KeyValuePair<Vector3, bool> entry in playerSpots)
        {
            Debug.Log(entry.Key);
            Debug.Log(entry.Value);
        }
        foreach (KeyValuePair<Vector3, bool> entry in playerSpots)
        {
            if (!entry.Value)
            {
                playerSpots[entry.Key] = true; //set spot to occupied
                spawnPoint = entry.Key;
                break;
            }
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), spawnPoint, Quaternion.identity);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        //remove player object
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(multiPlayerSceneIndex);
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
    /*
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiPlayerSceneIndex)
        {
            isGameLoaded = true;
            PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            RPC_CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {

    }*/
}
