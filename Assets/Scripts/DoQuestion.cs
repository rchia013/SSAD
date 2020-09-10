using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DoQuestion : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        // HARD CODED!!!

        originalColor = new Color(0, 0, 0, (29 / 255));
        
        b1.onClick.AddListener(Correct);
        b2.onClick.AddListener(Wrong);
        b3.onClick.AddListener(Wrong);
        b4.onClick.AddListener(Wrong);

        background = gameObject.GetComponent<Image>();
        background.color = originalColor;

        player = GameObject.FindWithTag(playerTag);
    }

    private void OnEnable()
    {
        background = gameObject.GetComponent<Image>();
        background.color = new Color(0, 0, 0, (29 / 255));
    }

    void Correct()
    {
        print("Correct");
        if (pointsAwardable)
            player.GetComponent<Movement>().ChangePoints(3);

        gameObject.SetActive(false);

        answered = true;
        correct = true;
    }

    void Wrong()
    {
        print("Wrong");

        if (pointsAwardable)
            player.GetComponent<Movement>().ChangePoints(-3);

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
}
