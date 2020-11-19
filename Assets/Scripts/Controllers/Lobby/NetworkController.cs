using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{

    /// <summary>
    /// Connect to Photon as configured in the PhotonServerSettings file
    /// </summary>
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// This is a callback function provided in the MonoBehaviourPunCallbacks class provided by PUN 2
    /// Called when the client is connected to the Master Server and ready for matchmaking and other tasks.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
    }
}
