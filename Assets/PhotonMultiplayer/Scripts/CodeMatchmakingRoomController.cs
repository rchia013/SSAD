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

<<<<<<< Updated upstream
=======
<<<<<<< Updated upstream
=======
    public Button startButton;

    public void Update()
    {
        startButton.interactable = readyToStart();
    }

    private bool readyToStart()
    {
        for (int i = 0; i < AvatarController.playerList.Count; i++)
        {
            if (AvatarController.playerList[PhotonNetwork.PlayerList[i].NickName] == -1)
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
>>>>>>> Stashed changes

>>>>>>> Stashed changes
    public override void OnJoinedRoom()
    {
        playerCount.text = "Players: "+ PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            gameObject.GetComponent<AvatarController>().addPlayer(PhotonNetwork.PlayerList[i].NickName, false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " Players";

        gameObject.GetComponent<AvatarController>().addPlayer(newPlayer.NickName, true);
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
