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

    public Image CD;
    public Image CDBG;

    public bool pointsAwardable;
    public bool answered = false; 
    public bool correct = false;

    public float timeLimit;
    private float curTime;

    public Image background;
    Color originalColor;

    GameObject player;
    public string playerTag;
    int playerIndex;
    private PhotonView PV;
    

    private QuestionManager QM;

    private Question question;
    

    // Start is called before the first frame update


    private void Start()
    {
        // Color Setup

        originalColor = background.color;

        // Player Information

        player = GameSetUp.GS.player;
        playerIndex = GameSetUp.GS.playerIndex;
        PV = player.GetComponent<PlayerController>().PV;

        // Setup Question Manager;

        QM = GameObject.FindWithTag("GameController").GetComponent<QuestionManager>();
        timeLimit = QM.getTimeLimit();

        setupNewQuestion();
    }

    private void Update()
    {
        if (QM.isEnded())
        {
            deactivateUI();
            this.enabled = false;
        }
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

        description.SetText(question.Description);
        string answer = question.Correct;
        int correctOption = Int32.Parse(answer);

        for (int i = 0; i < 4; i++)
        {
            int index = i;
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
                buttons[i].onClick.AddListener(delegate { Correct(index); });
            }
            else
            {
                buttons[i].onClick.AddListener(delegate { Wrong(index); });
            }
            
        }

        StartCoroutine("startCD");
    }

    private Question getQuestion()
    {
        if (QM != null)
        {
            return QM.getRandomQuestion();
        }
        else
        {
            return null;
        }
    }

    IEnumerator startCD()
    {
        float percentage;

        for (curTime = timeLimit; curTime > 0.0f; curTime -= Time.deltaTime)
        {
            percentage = curTime / timeLimit;

            if (percentage > 0.667)
            {
                CD.color = Color.green;
            }
            else if (percentage > 0.333)
            {
                CD.color = Color.yellow;
            }
            else
            {
                CD.color = Color.red;
            }

            CD.fillAmount = percentage;
            CDBG.fillAmount = percentage;
            yield return null;
        }

        if (!answered && !correct)
        {
            Unanswered();
        }
    }

    private void resetButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
    }

    void Correct(int index)
    {
        QM.recordResponse(question.ID, index);
        resetButtons();

        if (pointsAwardable)
        {
            int points = Mathf.RoundToInt(curTime + 0.49f);
            Debug.Log("Points"+ points);
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, points);
        }

        answered = true;
        correct = true;

        StartCoroutine("Disappear");
    }

    void Wrong(int index)
    {
        QM.recordResponse(question.ID, index);

        resetButtons();

       
        if (pointsAwardable)
        {
            int points = Mathf.FloorToInt(timeLimit / 3) * (-1);
            Debug.Log("Points"+(points));
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, points);
        }

        answered = true;
        correct = false;

        StartCoroutine("FailBGChange");
    }

    void Unanswered()
    {
        QM.recordResponse(question.ID, -1);
        resetButtons();

        answered = false;
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

        gameObject.SetActive(false);
    }

    IEnumerator Disappear()
    {
        Color color = background.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.75f)
        {
            Color newColor = new Color(Mathf.Lerp(color.r, 0.0f, t), Mathf.Lerp(color.g, 0.0f, t), Mathf.Lerp(color.b, 0.0f, t), Mathf.Lerp(color.a, 0.0f, t));

            background.color = newColor;

            yield return null;
        }

        gameObject.SetActive(false);
    }

    //void recordResponse(int playerIndex, int questionID, int resp)
    //{
    //    QM.recordResponse(playerIndex, question.ID, resp);
    //}

    void deactivateUI()
    {
        gameObject.SetActive(false);   
    }
}
