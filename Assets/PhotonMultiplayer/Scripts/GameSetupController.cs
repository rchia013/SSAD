using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviourPunCallbacks
{

    // Use this for initialization

    private int spawnIndex;

    /*void Start () {
		foreach (Player player in PhotonNetwork.PlayerList)
        {
			Debug.Log(player.NickName);
        }
        CreatePlayer();
	}
	
	// Update is called once per frame
	private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), Vector3.zero, Quaternion.identity);
    }*/
    // public static GameSetupController GS;

    public Transform[] spawnPoints;
    void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("HEREREREREREREREREREr");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("HERRE");
        Debug.Log(spawnIndex);
        Debug.Log(spawnPoints);
        spawnIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;
        Debug.Log(spawnIndex);
        /*if (spawnIndex >= spawnPoints.Length)
        {
            spawnIndex = 0;
        }*/
        // Debug.Log(spawnPoints[0].transform.position);
        // Debug.Log(spawnPoints[1].transform.position);
        Debug.Log(spawnPoints[spawnIndex].transform.position);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerTest"), spawnPoints[spawnIndex].transform.position, Quaternion.identity);
        //spawnIndex++;
    }
}
