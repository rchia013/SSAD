using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject joinButton;
    [SerializeField]
    private TextMeshProUGUI playerCount;
    [SerializeField]
    private Text playerCount2;
    [SerializeField]
    private int multiplayerSceneIndex;


    public override void OnJoinedRoom()
    {
        joinButton.SetActive(false);
        playerCount.text = "Players: "+ PhotonNetwork.PlayerList.Length;
        playerCount2.text = "Players: " + PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            gameObject.GetComponent<AvatarController>().addPlayer(PhotonNetwork.PlayerList[i].NickName);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().addPlayer(newPlayer.NickName);
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";
        playerCount2.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().removePlayer(otherPlayer.NickName);
    }

    
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
