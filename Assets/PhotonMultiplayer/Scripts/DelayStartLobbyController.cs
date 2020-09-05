using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject delayStartButton;
    [SerializeField]
    private GameObject delayCancelButton;
    [SerializeField]
    private GameObject loadingButton;
    [SerializeField]
    private int RoomSize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Here");
        delayStartButton.SetActive(true);
        loadingButton.SetActive(false);
    }

    public void DelayStart()
    {
        Debug.Log("Quick start");
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick start  2");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom();
    }

    public void DelayCancel()
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
