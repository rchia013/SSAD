using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// This script controls the logic of the 
/// </summary>
public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    // Player count display
    [SerializeField]
    private TextMeshProUGUI playerCount;

    // Multiplayer scene index
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

    /// <summary>
    /// Update is called every frame, and is the main function used to determine if the host can start the game
    /// </summary>
    public void Update()
    {
        startButton.interactable = readyToStart();
    }

    /// <summary>
    /// This is called to ensure that all the players in the Room have chosen an avatar, so that the host is able to start.
    /// </summary>
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

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class provided by PUN 2
    /// Called when the LoadBalancingClient entered a room, no matter if this client created it or simply joined.
    /// </summary>
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

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class provided by PUN 2
    /// Called when a remote player entered the room. 
    /// This Player is already added to the playerlist.
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // The player count is updated for each client
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        // This calls the addPlayer() function in the AvatarController class
        gameObject.GetComponent<AvatarController>().addPlayer(newPlayer.NickName, true);
    }

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class provided by PUN 
    /// Called when a remote player left the room or became inactive. 
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // The player count is updated for each client
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        // This calls the removePlayer() function in the AvatarController class
        gameObject.GetComponent<AvatarController>().removePlayer(otherPlayer.NickName);
    }

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class  provided by PUN 2 
    /// Called when the local user/client left a room, so the game's logic can clean up it's internal state.
    /// When leaving a room, the LoadBalancingClient will disconnect the Game Server and connect to the Master Server.
    /// </summary>
    public override void OnLeftRoom()
    {
        gameObject.SetActive(false);
        playerCount.text = "0 Players";

        gameObject.GetComponent<MapController>().resetMap();


        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("CodeMatchMakingMenuDemo");
    }
    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class  provided by PUN 2 
    /// Called when a previous OpJoinRoom call failed on the server
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// This function is triggered when the host clicks "Start". It checks that all the players in the Room
    /// have all chosen an avatar, before it loads the game.
    /// </summary>
    public void StartGameOnClick()
    {
        for (int i = 0; i < LobbySetUp.LS.playerList.Count; i++)
        {
            if (LobbySetUp.LS.playerList[PhotonNetwork.PlayerList[i].NickName] == -1)
            {
                return;
            }
        }

        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}
