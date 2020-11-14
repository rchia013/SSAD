using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// Initialise the Difficulty and Category buttons in the Create/Join Room screen
    /// </summary>
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

    /// <summary>
    /// When a category is selected, 
    /// </summary>
    /// <param name="index"></param>
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
        LobbySetUp.LS.category = cat;
    }

    /// <summary>
    /// When a difficuly is selected, the other difficulty levels cannot be selected.
    /// To select another difficulty level, you must click on the current one to unselect it first.
    /// </summary>
    /// <param name="index"></param>
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
        LobbySetUp.LS.difficulty = diff;
    }


    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class  provided by PUN 2 
    /// Called when the client is connected to the Master Server and ready for matchmaking and other tasks
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // This defines if all clients in a room should automatically load the same level as the Master Client.
        PhotonNetwork.AutomaticallySyncScene = true;

        MainPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        InitializeButtons();

        PhotonNetwork.JoinLobby();

        PhotonNetwork.NickName = Login.currentUser.username;
    }

    /// <summary>
    /// Join lobby when clicked. Set the panels accordingly.
    /// </summary>
    public void JoinLobbyOnClick()
    {
        MainPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// Detects when the room size input value changes and updates accordingly
    /// </summary>
    /// <param name="sizeIn"></param>
    public void OnRoomSizeInputChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }

    /// <summary>
    /// Create a room on click. It generates a 4 digit room code automatically and creates the room with the specified room options.
    /// The room options sets whether the room is visible in the lobby, is open to be joined, and the maximum number of players.
    /// </summary>
    public void CreateRoomOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PublishUserId = true;

        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();

        roomController.GetComponent<AvatarController>().enabled = true;
        roomController.GetComponent<AvatarController>().maxPlayers = roomSize;
        roomController.GetComponent<AvatarController>().isCreator = true;

        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);

        PhotonNetwork.CreateRoom(roomName, roomOps);

        codeDisplay.text = "Code: " + roomName;
    }

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class  provided by PUN 2 
    /// A callback for when the room creation failed, which can be due to an existing room with the same room code
    /// This prompts the creation of a room again.
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        roomOps.PublishUserId = true;

        int roomCode = Random.Range(1000, 10000);
        roomName = roomCode.ToString();
        PhotonNetwork.CreateRoom(roomName, roomOps);

        codeDisplay.text = roomName;
    }

    /// <summary>
    /// Cancels the room when clicked.
    /// </summary>
    public void CancelRoomOnClick()
    {
        // This requests each player client to disconnect if the requesting user is the master client.
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CloseConnection(player);
            }
        }
        // Leave the current room and return to the Master Server where you can join or create rooms
        PhotonNetwork.LeaveRoom();
        RoomPanel.SetActive(false);
    }

    // This is a function that is called when the code player by the input changes and updates the variable.
    public void OnCodeInputChanged(string code)
    {
        joinCode = code;
    }

    /// <summary>
    /// Joins the room when clicked, with a joinCode
    /// </summary>
    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(joinCode);
        codeDisplay.text = "Code: "+joinCode;
    }

    /// <summary>
    /// Leaves the room when clicked
    /// </summary>
    public void LeaveRoomOnClick()
    {
        // Checks whether the player is in the room, if so,
        // Leave the current room and return to the Master Server where you can join or create rooms
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class provided by PUN 2 
    /// Called when the local user/client left a room, so the game's logic can clean up it's internal state.
    /// </summary>
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

    public void backMainMenuOnClick()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main Menu");
    }
  
}
