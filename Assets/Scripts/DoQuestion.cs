using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using System;

public class DoQuestion : MonoBehaviour
{
    public TextMeshProUGUI description;

    Button[] buttons = new Button[4];

    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;

    public bool pointsAwardable;
    public bool answered = false; 
    public bool correct = false;

    Image background;
    Color originalColor;

    GameObject player;
    public string playerTag;
    int playerIndex;
    private PhotonView PV;

    private QuestionManager QM;

    private Question question;
    private int response;

    // Start is called before the first frame update


    private void Start()
    {
        // Color Setup

        originalColor = new Color(0, 0, 0, (29 / 255));

        // Player Information

        player = GameSetUp.GS.player;
        playerIndex = GameSetUp.GS.playerIndex;
        PV = player.GetComponent<Movement>().PV;

        // Setup Question Manager;

        QM = GameObject.FindWithTag("GameController").GetComponent<QuestionManager>();
        setupNewQuestion();

        background = gameObject.GetComponent<Image>();
        background.color = originalColor;
    }

    private void OnEnable()
    {
        // Setup UI

        setupNewQuestion();

        background.color = originalColor;

    }

    private void setupNewQuestion()
    {
        question = getQuestion();

        buttons[0] = b1;
        buttons[1] = b2;
        buttons[2] = b3;
        buttons[3] = b4;

        while (question.Description == null)
        {

        }
        description.SetText(question.Description);
        string answer = question.Correct;
        int correctOption = Int32.Parse(answer);

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(question.Options.one);
                    break;
                case 1:
                    buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(question.Options.two);
                    break;
                case 2:
                    buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(question.Options.three);
                    break;
                case 3:
                    buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(question.Options.four);
                    break;
            }
            if (correctOption == (i+1))
            {
                buttons[i].onClick.AddListener(delegate { Correct(i); });
            }
            else
            {
                buttons[i].onClick.AddListener(delegate { Wrong(i); });
            }
            
        }
    }

    private void resetButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
    }

    private Question getQuestion()
    {
        if (QM != null)
        {
            return QM.getRandomQuestion(playerIndex);
        }
        else
        {
            return null;
        }
    }


    void Correct(int index)
    {
        print("Correct");
        resetButtons();

        response = index;
        recordResponse();

        if (pointsAwardable)
        {
            Debug.Log("Points"+ 3);
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, 3);
        }
        gameObject.SetActive(false);

        answered = true;
        correct = true;
    }

    void Wrong(int index)
    {
        print("Wrong");
        resetButtons();

        response = index;
        recordResponse();

        if (pointsAwardable)
        {
            Debug.Log("Points"+(-3));
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, -3);
        }

        answered = true;
        correct = false;

        StartCoroutine("FailBGChange");
    }

    IEnumerator FailBGChange()
    {
        Color color = background.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newColor = new Color(Mathf.Lerp(color.r, 0.965f, t), Mathf.Lerp(color.g, 0.275f,t), Mathf.Lerp(color.b, 0.196f,t), Mathf.Lerp(color.a, 0.396f, t));

            background.color = newColor;

            yield return null;
        }
    }

    void recordResponse()
    {
        QM.recordResponse(playerIndex, question.ID, response);
    }
}
