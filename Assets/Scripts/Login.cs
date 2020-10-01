using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using Newtonsoft.Json;

public class Login : MonoBehaviour
{

    private string AuthKey = "AIzaSyBHzLV-p9Uw1iMVKdjPgg-50LoqtxOwvoM";
    private string databaseURL = "https://quizguyz.firebaseio.com/Users/";
    public static string localid;
    public string idToken;
    public string username;

    
    public TMP_InputField signInEmail;
    public TMP_InputField signInPassword;
    public TMP_InputField signUpEmail;
    public TMP_InputField signUpUsername;
    public TMP_InputField signUpPassword;

    public TextMeshProUGUI failLogin;

    private void Start()
    {
        failLogin.enabled = false;
    }


    // Apparently you need to have password length > 6 and cannot sign up with same email otherwise error
    public bool SignUpUser(string email, string name, string password)
    {
        bool success = true;
        string userData = "{\"email\":\""+ email +"\", \"password\":\""+password+"\"}";
        RestClient.Post(url: "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(onResolved: response =>
         {
             print(response.Text);
             SignResponse r = JsonConvert.DeserializeObject<SignResponse>(response.Text);
             localid = r.localid;
             idToken = r.idToken;
             username = name;
             success = PostToDatabase();
         }).Catch(error =>
        {
            Debug.Log(error);
            success = false;
        });
        return success;
    }

    public bool SignInUser(string email, string password)
    {
        bool success = true;
        string userData = "{\"email\":\"" + email + "\", \"password\":\"" + password + "\"}";
        RestClient.Post(url: "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(onResolved: response =>
        {
            print(response.Text);
            SignResponse r = JsonConvert.DeserializeObject<SignResponse>(response.Text);
            localid = r.localid;
            idToken = r.idToken;
        }).Catch(error =>
        {
            Debug.Log(error);
            success = false;
        });

        return success;
    }

    public void signUpButton()
    {
        bool signUpSuccess = SignUpUser(signUpEmail.text, signUpUsername.text, signUpPassword.text);
        print(signUpSuccess);
        if (localid!=null)
        {
            loadScene();
        }
        else
        {
            StartCoroutine("displayInvalidUser");

        }
    }



    public void signInButton()
    {
        bool loginSuccess = SignInUser(signInEmail.text, signInPassword.text);
        if (localid!=null)
        {
            loadScene();
        }
        else
        {
            //do some UI feedback
            StartCoroutine("displayInvalidUser");
        }
    }

    private bool PostToDatabase()
    {
        bool success = true;
        User user = new User(username, localid);
        RestClient.Put(url: "https://quizguyz.firebaseio.com/Users/"  + localid + ".json", user).Then(onResolved:response => {
        }).Catch(error => {
            Debug.Log(error);
            success = false;
        });
        print(success);
        return success;
    }

    IEnumerator displayInvalidUser()
    {
        failLogin.enabled = true;
        yield return new WaitForSeconds(1);
        failLogin.enabled = false;
    }

    public void loadScene()
    {
        SceneManager.LoadScene("CodeMatchMakingMenuDemo");
    }

}
