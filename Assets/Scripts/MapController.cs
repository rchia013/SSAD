using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Toggle map1;
    public Toggle map2;
    public Toggle map3;
    public Toggle map4;

    private List<Toggle> toggles = new List<Toggle>();

    private bool mapSelected = false;
    public static int mapIndex = -1;

    public GameObject MapPanel;
    public GameObject RoomPanel;

    public Image MapDisplay;
    public Image MapBorder;

    // Start is called before the first frame update
    void Start()
    {
        InitializeToggles();
    }

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

    public void CustomizeMapOnClick()
    {
        RoomPanel.SetActive(false);
        MapPanel.SetActive(true);
    }

    public void ConfirmMapOnClick()
    {
        MapPanel.SetActive(false);
        RoomPanel.SetActive(true);

        displaySelectedMap();
    }

    private void displaySelectedMap()
    {
        string mapPath = "";

        if (mapIndex == -1)
        {
            mapPath =  "Maps/Unknown";
            MapBorder.gameObject.SetActive(false);
        }
        else
        {
            mapPath = ("Maps/Map" + mapIndex);
            MapBorder.gameObject.SetActive(true);
        }

        MapDisplay.sprite = Resources.Load<Sprite>(mapPath);
    }
}
