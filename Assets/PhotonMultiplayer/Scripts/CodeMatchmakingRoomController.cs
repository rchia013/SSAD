using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject joinButton;
    [SerializeField]
    private Text playerCount;
    [SerializeField]
    private Text playerCount2;
    [SerializeField]
    private int multiplayerSceneIndex;


    public override void OnJoinedRoom()
    {
        joinButton.SetActive(false);
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
    }
    // Start is called before the first frame update
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";
    }

    // Update is called once per frame
    public override void OnLeftRoom()
    {
        playerCount.text = "0 Players";
        playerCount2.text = "0 Players";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void StartGameOnClick()
    {
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}
