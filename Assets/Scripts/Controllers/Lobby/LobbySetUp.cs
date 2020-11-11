using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class LobbySetUp : MonoBehaviour
{
    public static LobbySetUp LS;

    public Image[] platforms;

    public Image[] Avatars;
    public List<Image> CurrentAvatars = new List<Image>();

    public TextMeshProUGUI[] Names;
    public List<TextMeshProUGUI> CurrentNames = new List<TextMeshProUGUI>();

    public Dictionary<string, int> playerList = new Dictionary<string, int>();

    public int category;
    public int difficulty;

    public int mapIndex;

    private void OnEnable()
    {
        if (LobbySetUp.LS == null)
        {
            LobbySetUp.LS = this;
        }
    }
}
