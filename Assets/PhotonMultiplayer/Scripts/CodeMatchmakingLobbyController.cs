using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CodeMatchmakingLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject roomController;

    // Panels
    [SerializeField]
    private GameObject LobbyPanel;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject RoomPanel;

    // Code display
    [SerializeField]
    private TextMeshProUGUI codeDisplay;

    private string joinCode = null;
    private string roomName;
    private int roomSize = -1;

    // Buttons
    [SerializeField]
    private Button Join;
    [SerializeField]
    private Button Create;

    // UI:

    public Button math;
    public Button science;
    public Button geog;
    public Button general;

    private List<Button> buttonsCat = new List<Button>();
    private bool catChosen = false;
    public static int cat;

    public Button easy;
    public Button medium;
    public Button hard;

    private List<Button> buttonsDiff = new List<Button>();
    private bool diffChosen = false;
    public static int diff;

    private void Start()
    {
        LobbyPanel.SetActive(false);
    }

    private void Update()
    {
        if (catChosen && diffChosen && roomSize > 1 && roomSize <= 4)
        {
            Create.interactable = true;
        }
        else
        {
            Create.interactable = false;
        }

        if (joinCode != null && joinCode != "")
        {
            Join.interactable = true;
        }
        else
        {
            Join.interactable = false;
        }
    }

    private void InitializeButtons()
    {
        buttonsCat.Add(math);
        buttonsCat.Add(science);
        buttonsCat.Add(geog);
        buttonsCat.Add(general);

        for (int i = 0; i < buttonsCat.Count; i++)
        {
            int index = i;
            buttonsCat[i].onClick.AddListener(delegate { CatClicked(index); });
        }

        buttonsDiff.Add(easy);
        buttonsDiff.Add(medium);
        buttonsDiff.Add(hard);


        for (int i = 0; i < buttonsDiff.Count; i++)
        {
            int index = i;
            buttonsDiff[i].onClick.AddListener(delegate { DiffClicked(index); });
        }
    }

    void CatClicked(int index)
    {
        if (!catChosen)
        {
            for (int i = 0; i < buttonsCat.Count; i++)
            {
                if (i != index)
                {
                    buttonsCat[i].interactable = false;
                }
            }
            cat = index;
            catChosen = true;
        }

        else
        {
            for (int i = 0; i < buttonsCat.Count; i++)
            {
                if (i != index)
                {
                    buttonsCat[i].interactable = true;
                }
            }
            cat = -1;
            catChosen = false;
        }

        print("Category = " + cat);
    }

    void DiffClicked(int index)
    {
        if (!diffChosen)
        {
            for (int i = 0; i < buttonsDiff.Count; i++)
            {
                if (i != index)
                {
                    buttonsDiff[i].interactable = false;
                }
            }
            diff = index + 1;
            diffChosen = true;
        }

        else
        {
            for (int i = 0; i < buttonsDiff.Count; i++)
            {
                if (i != index)
                {
                    buttonsDiff[i].interactable = true;
                }
            }
            diff = -1;
            diffChosen = false;
        }
        print("Difficulty = " + diff);
    }



    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        MainPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        InitializeButtons();

        PhotonNetwork.JoinLobby();

        PhotonNetwork.NickName = Login.currentUser.username;
    }

    public void JoinLobbyOnClick()
    {
        MainPanel.SetActive(false);
        LobbyPanel.SetActive(true);
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
        Debug.Log("Creating room now");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PublishUserId = true;

        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();

        roomController.GetComponent<AvatarController>().enabled = true;
        roomController.GetComponent<AvatarController>().maxPlayers = roomSize;
        roomController.GetComponent<AvatarController>().isCreator = true;

        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);

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
        codeDisplay.text = "Code: "+joinCode;

        //if (!roomController.GetComponent<AvatarController>().isCreator)
        //{
        //    roomController.GetComponent<AvatarController>().isCreator = false;
        //}
        // roomController.GetComponent<AvatarController>().enabled = true;

        // lobbyPanel.SetActive(false);
        // RoomPanel.SetActive(true);
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
        MainPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
