using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomController room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    private void Awake()
    {
        if (PhotonRoomController.room == null)
        {
            PhotonRoomController.room = this;
        }
        else
        {
            if (PhotonRoomController.room != this)
            {
                Destroy(PhotonRoomController.room.gameObject);
                PhotonRoomController.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
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
    }*/

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

    }
}
