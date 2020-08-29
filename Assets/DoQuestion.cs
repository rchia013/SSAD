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

    public bool? correct = null;

    Image background;
    Color originalColor;

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
    }

    private void OnEnable()
    {
        background.color = originalColor;
    }

    void Correct()
    {
        print("Correct");
        gameObject.SetActive(false);

        correct = true;
    }

    void Wrong()
    {
        print("Wrong");

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
