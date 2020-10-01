using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Photon.Pun;
using Photon.Realtime;

public class AvatarController : MonoBehaviour
{

    public int NumPlayers;

    public bool isCreator = false;

    public Button startButton;
    public Button cancelButton;
    public Button leaveButton;


    //Room Page:

    public Image platform1;
    public Image platform2;
    public Image platform3;
    public Image platform4;

    public Image Avatar1;
    public Image Avatar2;
    public Image Avatar3;
    public Image Avatar4;

    private List<Image> Avatars = new List<Image>();

    public TextMeshProUGUI Name1;
    public TextMeshProUGUI Name2;
    public TextMeshProUGUI Name3;
    public TextMeshProUGUI Name4;

    private List<TextMeshProUGUI> Names = new List<TextMeshProUGUI>();

    public GameObject RoomPanel;
    public GameObject AvatarPanel;
    public Button customize;

    //Customize Page:

    public Toggle char1;
    public Toggle char2;
    public Toggle char3;

    private List<Toggle> toggles = new List<Toggle>();

    public Button blue;
    public Button pink;
    public Button green;
    public Button yellow;
    public Button purple;
    public Button orange;

    private List<Button> buttons = new List<Button>();

    private Dictionary<int, bool> colorTaken = new Dictionary<int, bool>();

    public Button confirm;

    //Store User (username) of p1-4 and Character selection:

    public static Dictionary<string, int> playerList = new Dictionary<string, int>();


    // Current User Info:

    private int curSelection;
    public Image curAvatar;
    private bool colorSelected = false;
    private bool charSelected = false;

    public PhotonView PV;

    private void OnEnable()
    {
        // HARD CODED:
        NumPlayers = 3;//PhotonNetwork.CurrentRoom.MaxPlayers;
        //

        PV = GetComponent<PhotonView>();

        if (NumPlayers >= 1)
        {
            platform1.gameObject.SetActive(true);
            Avatars.Add(Avatar1);
            Names.Add(Name1);
        }
        if (NumPlayers >= 2)
        {
            platform2.gameObject.SetActive(true);
            Avatars.Add(Avatar2);
            Names.Add(Name2);
        }
        if (NumPlayers >= 3)
        {
            platform3.gameObject.SetActive(true);
            Avatars.Add(Avatar3);
            Names.Add(Name3);
        }
        if (NumPlayers >= 4)
        {
            platform4.gameObject.SetActive(true);
            Avatars.Add(Avatar4);
            Names.Add(Name4);
        }

        for (int i = 0; i < Avatars.Count; i++)
        {
            Avatars[i].sprite = null;
            Avatars[i].color = Color.clear;
        }

        // Initialize Avatar Page:

        InitializeButtons();
        InitializeToggles();
    }

    private void Update()
    {
        if (isCreator)
        {
            startButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }
        else
        {
            leaveButton.gameObject.SetActive(true);
        }
    }

    //Handle change of Players:

    //public void syncPlayerList()
    //{
    //    if (isCreator)
    //    {
    //        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //        {
    //            updateAvatar(PhotonNetwork.PlayerList[i].NickName, playerList[PhotonNetwork.PlayerList[i].NickName]);
    //        }
    //    }
    //}

    public void addPlayer(string newUsername)
    {
        playerList.Add(newUsername, -1);

        PV.RPC("updateTotalUI", RpcTarget.All);

        PV.RPC("updateAvatar", RpcTarget.All, Login.currentUser.username, curSelection);
        //PV.RPC("addP", RpcTarget.All, newUsername);
    }

    [PunRPC]
    private void addP(string newUsername)
    {
        playerList.Add(newUsername, -1);

        PV.RPC("updateTotalUI", RpcTarget.All);


    }

    public void removePlayer(string oldUsername)
    {
        playerList.Remove(oldUsername);

        PV.RPC("updateTotalUI", RpcTarget.All);
        //PV.RPC("removeP", RpcTarget.All, oldUsername);
    }

    [PunRPC]
    private void removeP(string oldUsername)
    {
        playerList.Remove(oldUsername);

        PV.RPC("updateTotalUI", RpcTarget.All);
    }

    //Handle Change of Avatars:

    [PunRPC]
    public void updateAvatar(string userName, int picIndex)
    {
        if (playerList[userName] != -1)
        {
            int oldColorIndex = playerList[userName] % 10;
            colorTaken[oldColorIndex] = false;

        }

        playerList[userName] = picIndex;

        int colorIndex = picIndex % 10;
        colorTaken[colorIndex] = true;

        PV.RPC("updateTotalUI", RpcTarget.All);
    }


    //Handle any state Change to UI:

    [PunRPC]
    void updateTotalUI()
    {
        int i = 0;

        foreach (KeyValuePair<string, int> player in playerList)
        {
            // Set Name:

            Names[i].SetText(player.Key);

            // Set Avatar:

            displayAvatar(Avatars[i], player.Value);

            i++;
        }
    }

    void displayAvatar(Image avatar, int selection)
    {
        String avatarPath = findAvatarPath(selection);

        if (avatarPath.Contains("Unknown"))
        {
            avatar.rectTransform.sizeDelta = new Vector2(10, 12);
            avatar.color = Color.white;
        }

        else if (avatarPath.Contains("Mummy"))
        {
            avatar.rectTransform.sizeDelta = new Vector2(15, 20);
            avatar.color = new Color(0.8f, 0.8f, 0.8f);

        }

        avatar.sprite = Resources.Load<Sprite>(avatarPath);
    }

    // Page Navigation:

    public void CustomizeCharacterOnClick()
    {
        RoomPanel.SetActive(false);
        AvatarPanel.SetActive(true);
    }

    public void ConfirmCharacterOnClick()
    {
        if (colorTaken[curSelection % 10])
        {
            print("Color Taken!");
        }
        else
        {
            AvatarPanel.SetActive(false);
            RoomPanel.SetActive(true);


            PV.RPC("updateAvatar", RpcTarget.All, Login.currentUser.username, curSelection);
        }
    }


    //Initialize Buttons/Toggles:

    private void InitializeButtons()
    {
        buttons.Add(blue);
        buttons.Add(pink);
        buttons.Add(green);
        buttons.Add(yellow);
        buttons.Add(purple);
        buttons.Add(orange);

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;

            buttons[i].onClick.AddListener(delegate { ColorClicked(index); });

            colorTaken.Add(i, false);
        }
    }

    private void InitializeToggles()
    {
        toggles.Add(char1);
        toggles.Add(char2);
        toggles.Add(char3);

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;

            toggles[i].onValueChanged.AddListener(delegate { CharClicked(index); });
        }
    }

    //Button/Toggle Handling (Color + Character):

    void CharClicked(int index)
    {
        if (!charSelected)
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                if (i != index)
                {
                    toggles[i].interactable = false;
                }
            }
            curSelection += (index * 10);
            charSelected = true;
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
            curSelection -= (index * 10);
            charSelected = false;
        }

        print(curSelection);
        displayAvatar(curAvatar, curSelection);
    }

    void ColorClicked(int index)
    {
        if (!colorSelected)
        {           
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i != index)
                {
                    buttons[i].interactable = false;
                }
            }

            buttons[index].gameObject.GetComponent<ColorSelect>().outlineActive(true);
            curSelection += (index + 1);
            colorSelected = true;
        }

        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i != index)
                {
                    buttons[i].interactable = true;
                }
            }

            buttons[index].gameObject.GetComponent<ColorSelect>().outlineActive(false);
            curSelection -= (index + 1);
            colorSelected = false;
        }

        print(curSelection);
        displayAvatar(curAvatar, curSelection);
    }

    private string findAvatarPath(int selection)
    {
        if (selection == -1)
        {
            return "Avatars/Unknown";
        }

        int chosenCharacter = -1;
        int chosenColor = -1;

        string avatarPath = "Avatars/";

        chosenCharacter = selection / 10;
        chosenColor = selection % 10;

        switch (chosenCharacter)
        {
            case 0:
                avatarPath += "Mummy_";
                break;

            case 1:
                avatarPath += "/////1";
                break;

            case 2:
                avatarPath += "/////2";
                break;

            default:
                return "Avatars/Unknown";
        }

        switch (chosenColor)
        {
            case 1:
                avatarPath += "Blue";
                break;

            case 2:
                avatarPath += "Pink";
                break;

            case 3:
                avatarPath += "Green";
                break;

            case 4:
                avatarPath += "Yellow";
                break;

            case 5:
                avatarPath += "Purple";
                break;

            case 6:
                avatarPath += "Orange";
                break;

            default:
                return "Avatars/Unknown";
        }

        return avatarPath;
    } 
}
