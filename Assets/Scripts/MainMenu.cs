using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject ProfilePanel;
    public GameObject LeaderboardPanel;
    public GameObject AchievementsPanel;

    public void playGame()
    {
        SceneManager.LoadScene("CodeMatchMakingMenuDemo");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void OpenProfileOnClick()
    {
        MenuPanel.SetActive(false);
        ProfilePanel.SetActive(true);
    }

    public void CloseProfileOnClick()
    {
        ProfilePanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void OpenLeaderBoardeOnClick()
    {
        MenuPanel.SetActive(false);
        LeaderboardPanel.SetActive(true);
    }

    public void CloseLeaderBoardOnClick()
    {
        LeaderboardPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void OpenAchievementsOnClick()
    {
        MenuPanel.SetActive(false);
        AchievementsPanel.SetActive(true);
    }

    public void CloseAchievementsOnClick()
    {
        AchievementsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

}
