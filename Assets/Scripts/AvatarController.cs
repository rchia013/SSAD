using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Photon.Pun;
using Proyecto26;
using Newtonsoft.Json;
using Photon.Realtime;

public class AvatarController : MonoBehaviour
{

    public bool isCreator = false;



    public GameObject RoomPanel;
    public GameObject AvatarPanel;
    public Button customize;

    //Customize Page:

    public Toggle char1;
    public Toggle char2;
    public Toggle char3;

    public GameObject blockChar2;
    public GameObject blockChar3;


    private List<Toggle> toggles = new List<Toggle>();

    public Button blue;
    public Button pink;
    public Button green;
    public Button yellow;
    public Button purple;
    public Button brown;

    private List<Button> buttons = new List<Button>();

    private Dictionary<int, bool> colorTaken = new Dictionary<int, bool>();

    public Button confirm;

    //Store User (username) of p1-4 and Character selection:

    private Dictionary<string, int> playerList = new Dictionary<string, int>();
    
    public int maxPlayers = -1;
    private bool platformsInitialized = false;


    // Current User Info:

    private int curSelection;
    public Image curAvatar;
    private bool colorSelected = false;
    private bool charSelected = false;

    public PhotonView PV;

    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();

        playerList = LobbySetUp.LS.playerList;

        Achievement playerinfo = new Achievement();
        string playerurl = "https://quizguyz.firebaseio.com/Users/" + Login.localid;

        RestClient.Get(url: playerurl + ".json").Then(onResolved: response =>
        {
            //Get
            playerinfo = JsonConvert.DeserializeObject<Achievement>(response.Text);

            InitializeButtons();
            InitializeToggles(playerinfo.achievementPoints);

        });
    }

    private void Update()
    {
        if (selectionValid(curSelection))
        {
            confirm.interactable = true;
        }
        else
        {
            confirm.interactable = false;
        }
    }

    //Handle change of Players:

    public void addPlayer(string newUsername, bool selfSync)
    {
        LobbySetUp.LS.playerList.Add(newUsername, -1);

        PV.RPC("updateTotalUI", RpcTarget.All);

        if (selfSync)
        {
            PV.RPC("updateAvatar", RpcTarget.All, Login.currentUser.username, curSelection);
        }
    }
    
    [PunRPC]
    private void addP(string newUsername)
    {
        playerList.Add(newUsername, -1);

        PV.RPC("updateTotalUI", RpcTarget.All);
    }

    public void removePlayer(string oldUsername)
    {
        LobbySetUp.LS.playerList.Remove(oldUsername);

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
        Debug.Log("update total ui");
        if (LobbySetUp.LS.CurrentNames.Count > 0)
        {
            foreach (KeyValuePair<string, int> player in playerList)
            {
                // Set Name:
                LobbySetUp.LS.CurrentNames[i].SetText(player.Key);

                // Set Avatar:
                displayAvatar(LobbySetUp.LS.CurrentAvatars[i], player.Value);

                i++;
            }
            for (int j = i; j < LobbySetUp.LS.CurrentNames.Count; j++)
            {
                //Delete name
                LobbySetUp.LS.CurrentNames[j].SetText("");

                //Delete avatar
                destroyAvatar(LobbySetUp.LS.CurrentAvatars[j]);
            }
        }
    }
    
    void destroyAvatar(Image avatar)
    {
        avatar.sprite = null;
        Color c = avatar.color;
        c.a = 0;
        avatar.color = c;
    }

    public void displayAvatar(Image avatar, int selection)
    {
        String avatarPath = findAvatarPath(selection);

        if (avatarPath.Contains("Unknown"))
        {
            avatar.rectTransform.localPosition = new Vector3(0, 7.4f, 0);
            avatar.rectTransform.sizeDelta = new Vector2(10, 12);
            avatar.color = Color.white;
        }

        else if (avatarPath.Contains("Mummy"))
        {
            avatar.rectTransform.localPosition = new Vector3(0, 7.4f, 0);
            avatar.rectTransform.sizeDelta = new Vector2(15, 18);
            avatar.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if (avatarPath.Contains("Astronaut"))
        {
            avatar.rectTransform.localPosition = new Vector3(0, 7.4f, 0);
            avatar.rectTransform.sizeDelta = new Vector2(17, 22);
            avatar.color = new Color(0.92f, 0.92f, 0.92f);
        }
        else if (avatarPath.Contains("Ball"))
        {
            avatar.rectTransform.localPosition = new Vector3(0, 5.5f, 0);
            avatar.rectTransform.sizeDelta = new Vector2(14, 18);
            avatar.color = new Color(0.85f, 0.85f, 0.85f);
        }

        avatar.sprite = Resources.Load<Sprite>(avatarPath);
    }

    // Page Navigation:

    public void CustomizeCharacterOnClick()
    {
        RoomPanel.SetActive(false);
        AvatarPanel.SetActive(true);

        if (!selectionValid(curSelection))
        {
            updateAvailableColors();
        }
    }

    public void ConfirmCharacterOnClick()
    {
        if (colorTaken[curSelection % 10] && ((curSelection % 10) != (playerList[Login.currentUser.username] % 10)))
        {
            updateAvailableColors();
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
        buttons.Add(brown);

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;

            buttons[i].onClick.AddListener(delegate { ColorClicked(index); });

            colorTaken.Add((i+1), false);
        }
    }

    private void InitializeToggles(int points)
    {
        if (points < 100)
        {
            toggles.Add(char1);
            disableToggle(2);
            disableToggle(3);
        }
        else if (points < 200)
        {
            toggles.Add(char1);
            toggles.Add(char2);

            disableToggle(3);
        }
        else
        {
            toggles.Add(char1);
            toggles.Add(char2);
            toggles.Add(char3);
        }
        

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;

            toggles[i].onValueChanged.AddListener(delegate { CharClicked(index); });
        }
    }

    private void disableToggle(int index)
    {
        if (index == 2)
        {
            blockChar2.SetActive(true);
            char2.interactable = false;
        }
        else if (index == 3)
        {
            blockChar3.SetActive(true);
            char3.interactable = false;
        }
        
    }

    private void updateAvailableColors()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (colorTaken[i + 1] && ((curSelection % 10) != (i+1)))
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }
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
            curSelection += ((index + 1) * 10);
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
            curSelection -= ((index+1) * 10);
            charSelected = false;
        }
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

            curSelection += (index + 1);
            colorSelected = true;
        }

        else
        {
            updateAvailableColors();

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
            case 1:
                avatarPath += "Mummy_";
                break;

            case 2:
                avatarPath += "Astronaut_";
                break;

            case 3:
                avatarPath += "Ball_";
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
                avatarPath += "Brown";
                break;

            default:
                return "Avatars/Unknown";
        }

        return avatarPath;
    }

    private bool selectionValid(int sel)
    {
        if ((sel/10)>=1 && (sel % 10) >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
