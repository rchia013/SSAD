using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeMatchmakingLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyConnectButton;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private InputField playerNameInput;

    public GameObject roomController;

    private string roomName;
    private int roomSize;

    [SerializeField]
    private GameObject RoomPanel;
    [SerializeField]
    private TextMeshProUGUI codeDisplay;


    private string joinCode;
    [SerializeField]
    private GameObject joinButton;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();

        PhotonNetwork.NickName = Login.currentUser.username;
    }

    public void JoinLobbyOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    // CREATOR:

    public void OnRoomSizeInputChanged(string sizeIn)
    {
        Debug.Log(sizeIn);
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoomOnClick()
    {
        roomController.GetComponent<AvatarController>().isCreator = true;
        roomController.GetComponent<AvatarController>().enabled = true;
        lobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);

        Debug.Log("Creating room now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PublishUserId = true;

        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();

        Debug.Log(roomName);
        PhotonNetwork.CreateRoom(roomName, roomOps);

        codeDisplay.text = "Code: " + roomName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed since same name");

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PublishUserId = true;

        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();
        PhotonNetwork.CreateRoom(roomName, roomOps);

        codeDisplay.text = roomName;
    }

    public void CancelRoomOnClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CloseConnection(player);
            }
        }
        PhotonNetwork.LeaveRoom();
        RoomPanel.SetActive(false);
        joinButton.SetActive(true);
    }

    // JOINERS:

    public void OnCodeInputChanged(string code)
    {
        print("Code = " + code);
        joinCode = code;
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(joinCode);
        //if (!roomController.GetComponent<AvatarController>().isCreator)
        //{
        //    roomController.GetComponent<AvatarController>().isCreator = false;
        //}
        roomController.GetComponent<AvatarController>().enabled = true;

        lobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
    }

    public void LeaveRoomOnClick()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
    }

    public void MatchmakingCancelOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
