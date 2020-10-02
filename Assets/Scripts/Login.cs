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
    public string username = null;
    public bool? success = null;
    public bool usernameGet = false; 
    public bool displayFail = false;
    public bool? signUpSuccess = null;
    public static User currentUser;
    public static bool? emailInUse = null;

    
    public TMP_InputField signInEmail;
    public TMP_InputField signInPassword;
    public TMP_InputField signUpEmail;
    public TMP_InputField signUpUsername;
    public TMP_InputField signUpPassword;
    
    public TextMeshProUGUI failLogin;


    private void Update()
    {
        //print("signupsuccess in update = " + signUpSuccess);
        if (success == true && usernameGet)
        {
            loadScene();
        }
        else if (success == false && !displayFail)
        {
            StartCoroutine("displayInvalidUser");
        }
    }

    // Apparently you need to have password length > 6 and cannot sign up with same email otherwise error
    public void SignUpUser(string email, string name, string password)
    {
        string userData = "{\"email\":\""+ email +"\", \"password\":\""+password+"\"}";
        RestClient.Post(url: "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then(onResolved: response =>
         {
             SignResponse r = JsonConvert.DeserializeObject<SignResponse>(response.Text);
             localid = r.localid;
             idToken = r.idToken;
             username = name;
             PostToDatabase();
         }).Catch(error =>
        {
            Debug.Log("error=" + error);
            emailInUse = true;
            signUpSuccess = false;
        });
    }


    public void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\", \"password\":\"" + password + "\"}";
        RestClient.Post(url: "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(onResolved: response =>
        {
            //print(response.Text);
            SignResponse r = JsonConvert.DeserializeObject<SignResponse>(response.Text);
            localid = r.localid;
            idToken = r.idToken;
            success = true;
            getUsername(localid);
        }).Catch(error =>
        {
            Debug.Log(error);
            success = false;
        });


    }

    public void signUpButton()
    {
        SignUpUser(signUpEmail.text, signUpUsername.text, signUpPassword.text);
    }



    public void signInButton()
    {
        SignInUser(signInEmail.text, signInPassword.text);  
    }

    private void PostToDatabase()
    {
        User user = new User(username, localid);
        RestClient.Put(url: "https://quizguyz.firebaseio.com/Users/"  + localid + ".json", user).Then(onResolved:response => {
            currentUser = user;
            loadScene();
        }).Catch(error => {
            Debug.Log(error);
            signUpSuccess = false;
        });
    }

    IEnumerator displayInvalidUser()
    {
        displayFail = true;
        failLogin.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        failLogin.gameObject.SetActive(false);
        displayFail = false;
        success = null;
    }


    public void loadScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void getUsername(string userid)
    { 
        print("USERID! = " + userid); 
        RestClient.Get(url: "https://quizguyz.firebaseio.com/Users/" + userid + ".json").Then(onResolved: response =>
        {
            User user = JsonConvert.DeserializeObject<User>(response.Text);
            username = user.username;
            print("username = " + username);
            currentUser = new User(username, localid);
            usernameGet = true;
        });     
    }
}
