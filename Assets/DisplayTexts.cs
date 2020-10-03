using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class DisplayTexts : MonoBehaviour
{
    public TextMeshProUGUI passwordisTooShort;
    public TextMeshProUGUI emailInUse;

    public TMP_InputField signUpEmail;
    public TMP_InputField signUpPassword;

    public void passwordTooShort()
    {
        if(signUpPassword.text.Length < 6)
        {
            StartCoroutine("passwordisToooShort");
        }
        else if (Login.emailInUse == true)
        {
            StartCoroutine("emailisInUse");
        }
    }
    
    IEnumerator passwordisToooShort()
    {
        passwordisTooShort.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        passwordisTooShort.gameObject.SetActive(false);
    }
    public IEnumerator emailisInUse()
    {
        emailInUse.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        emailInUse.gameObject.SetActive(false);
    }

}
