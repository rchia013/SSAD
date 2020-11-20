using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// This script processes all logic related to map selection of the host of the room
/// </summary>
public class MapController : MonoBehaviour
{
    // Toggles to select map
    public Toggle map1;
    public Toggle map2;
    public Toggle map3;
    public Toggle map4;

    private List<Toggle> toggles = new List<Toggle>();

    // Variables to store map settings
    private bool mapSelected = false;
    public static int mapIndex = -1;

    // Variables storing the category and difficulty of the room
    public static int Category;
    public static int Difficulty;

    // Stores the UI panels
    public GameObject MapPanel;
    public GameObject RoomPanel;

    // Button to confirm map selected
    public Button ConfirmMap;

    // Image that renders display of map
    public Image MapDisplay;
    public Image MapBorder;

    private PhotonView PV;

    /// <summary>
    /// Start is called before the first frame update to initialise variables
    /// </summary>
    void Start()
    {
        PV = GetComponent<PhotonView>();
        InitializeToggles();

        resetMap();
    }

    /// <summary>
    ///  Update is called every frame and checks if a map has been selected. 
    ///  If a map has been selected, set the confirm button to be interactable and
    ///  display the selected map
    /// </summary>
    private void Update()
    {
        if (mapSelected)
        {
            ConfirmMap.interactable = true;
        }
        else
        {
            ConfirmMap.interactable = false;
        }
        displaySelectedMap();
    }

    /// <summary>
    /// INitialise the toggles to be able to enable the player to select the map
    /// </summary>
    private void InitializeToggles()
    {
        toggles.Add(map1);
        toggles.Add(map2);
        toggles.Add(map3);
        toggles.Add(map4);

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;

            toggles[i].onValueChanged.AddListener(delegate { MapClicked(index); });
        }
    }

    /// <summary>
    /// This function will be called when a map has been selected, which will disable the
    /// other map toggles. To select another map, the player has to click on the current map
    /// toggle (to unselect it) before selecting the other map.
    /// </summary>
    /// <param name="index"></param>
    void MapClicked(int index)
    {
        if (!mapSelected)
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                if (i != index)
                {
                    toggles[i].interactable = false;
                }
            }

            mapIndex = index;
            mapSelected = true;
        }

        else
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                if (i != index)
                {
                    toggles[i].interactable = true;
                }
            }
            mapIndex = -1;
            mapSelected = false;
        }
    }

    /// <summary>
    /// This displays the map panel when the host clicks on the customize map button
    /// </summary>
    public void CustomizeMapOnClick()
    {
        RoomPanel.SetActive(false);
        MapPanel.SetActive(true);
    }

    /// <summary>
    /// After confirming the map, the panels are set to inactive and active respectively to transition back to the main room page
    /// PV.RPC calls the PunRPC function setMapSettings
    /// </summary>
    public void ConfirmMapOnClick()
    {
        MapPanel.SetActive(false);
        RoomPanel.SetActive(true);
        PV.RPC("setMapSettings", RpcTarget.All, mapIndex, CodeMatchmakingLobbyController.cat, CodeMatchmakingLobbyController.diff);
        LobbySetUp.LS.mapIndex = mapIndex; 
        displaySelectedMap();
    }

    /// <summary>
    /// This sets the map settings for other clients in the room as well.
    /// PunRPC enables method-calls on remote clients in the same room.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="cat"></param>
    /// <param name="diff"></param>
    [PunRPC]
    private void setMapSettings(int map, int cat, int diff)
    {
        mapIndex = map;
        Category = cat;
        Difficulty = diff;
    }

    /// <summary>
    /// This loads the picture of the selected map and displays it
    /// </summary>
    private void displaySelectedMap()
    {
        string mapPath = "";

        if (mapIndex == -1)
        {
            mapPath = "Maps/Unknown";
            MapBorder.gameObject.SetActive(false);
        }
        else
        {
            mapPath = ("Maps/Map" + mapIndex);
            MapBorder.gameObject.SetActive(true);
        }

        MapDisplay.sprite = Resources.Load<Sprite>(mapPath);
    }

    /// <summary>
    /// Resets the map
    /// </summary>
    public void resetMap()
    {
        mapIndex = -1;
    }
}