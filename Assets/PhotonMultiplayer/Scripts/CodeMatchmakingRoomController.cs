using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI playerCount;
    [SerializeField]
    private int multiplayerSceneIndex;

    // Panels
    [SerializeField]
    private GameObject LobbyPanel;
    [SerializeField]
    private GameObject RoomPanel;

    // Buttons
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button leaveButton;
    [SerializeField]
    private Button mapButton;

    public void Update()
    {
        startButton.interactable = readyToStart();
    }

    private bool readyToStart()
    {
        if (LobbySetUp.LS.playerList.Count == PhotonNetwork.PlayerList.Length)
        {
            for (int i = 0; i < LobbySetUp.LS.playerList.Count; i++)
            {
                if (LobbySetUp.LS.playerList[PhotonNetwork.PlayerList[i].NickName] == -1)
                {
                    return false;
                }
            }

            if (MapController.mapIndex == -1)
            {
                return false;
            }

            return true;
        }

        return true;
    }

    public override void OnJoinedRoom()
    {

        GetComponent<AvatarController>().enabled = true;

        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);

        playerCount.text = "Players: " + PhotonNetwork.PlayerList.Length;


        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            LobbySetUp.LS.platforms[i].gameObject.SetActive(true);
            LobbySetUp.LS.CurrentAvatars.Add(LobbySetUp.LS.Avatars[i]);
            LobbySetUp.LS.CurrentNames.Add(LobbySetUp.LS.Names[i]);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            mapButton.gameObject.SetActive(true);
            mapButton.interactable = true;
        }
        else
        {
            leaveButton.gameObject.SetActive(true);
            mapButton.gameObject.SetActive(true);
            mapButton.interactable = false;
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            gameObject.GetComponent<AvatarController>().addPlayer(PhotonNetwork.PlayerList[i].NickName, false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().addPlayer(newPlayer.NickName, true);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().removePlayer(otherPlayer.NickName);
    }

    
    public override void OnLeftRoom()
    {
        gameObject.SetActive(false);
        playerCount.text = "0 Players";

        gameObject.GetComponent<MapController>().resetMap();


        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("CodeMatchMakingMenuDemo");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void StartGameOnClick()
    {
        for (int i = 0; i < LobbySetUp.LS.playerList.Count; i++)
        {
            if (LobbySetUp.LS.playerList[PhotonNetwork.PlayerList[i].NickName] == -1)
            {
                print("Everyone must select a Character!");
                return;
            }
        }

        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}
