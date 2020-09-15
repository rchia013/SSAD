using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

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

    private string roomName;
    private int roomSize;

    [SerializeField]
    private GameObject CreatePanel;
    [SerializeField]
    private InputField codeDisplay;

    [SerializeField]
    private GameObject joinPanel;
    [SerializeField]
    private InputField codeInputField;
    private string joinCode;
    [SerializeField]
    private GameObject joinButton;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true);

        if (PlayerPrefs.HasKey("NickName"))
        {
            if (PlayerPrefs.GetString("Nickname") == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
            } 
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            }
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
        }
        playerNameInput.text = PhotonNetwork.NickName;
    }

    public void PlayerNameUpdateInputChanged(string nameInput)
    {
        Debug.Log("HELLO");
        Debug.Log(nameInput);
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    public void JoinLobbyOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public void OnRoomSizeInputChanged(string sizeIn)
    {
        Debug.Log(sizeIn);
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoomOnClick()
    {
        CreatePanel.SetActive(true);

        Debug.Log("Creating room now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();

        Debug.Log(roomName);
        PhotonNetwork.CreateRoom(roomName, roomOps);

        codeDisplay.text = roomName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed since same name");

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };

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
        CreatePanel.SetActive(false);
        joinButton.SetActive(true);
    }

    public void OpenJoinPanel()
    {
        joinPanel.SetActive(true);
    }
    
    public void OnCodeInputChanged(string code)
    {
        Debug.Log(code);
        joinCode = code;
    }

    public void JoinRoomOnClick()
    {
        Debug.Log(joinCode);
        PhotonNetwork.JoinRoom(joinCode);
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
        joinButton.SetActive(true);
        joinPanel.SetActive(false);
        codeInputField.text = "";
    }

    public void MatchmakingCancelOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
