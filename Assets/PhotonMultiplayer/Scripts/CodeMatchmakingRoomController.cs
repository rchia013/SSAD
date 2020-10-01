using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeMatchmakingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI playerCount;
    [SerializeField]
    private int multiplayerSceneIndex;

    public override void OnJoinedRoom()
    {
        playerCount.text = "Players: "+ PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {

            gameObject.GetComponent<AvatarController>().addPlayer(PhotonNetwork.PlayerList[i].NickName);
        }

        gameObject.GetComponent<AvatarController>().syncPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().addPlayer(newPlayer.NickName);
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
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void StartGameOnClick()
    {
        for (int i = 0; i < AvatarController.playerList.Count; i++)
        {
            if (AvatarController.playerList[PhotonNetwork.PlayerList[i].NickName] == -1)
            {
                print("Everyone must Choose!");
                return;
            }
        }

        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}
