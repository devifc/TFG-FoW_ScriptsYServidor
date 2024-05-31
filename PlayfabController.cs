using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabController : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, privacyPolicyPanel, recoverUserPanel, notificationPanel;
    public InputField loginEmail, loginPassword,
        signupEmail, signupPassword, signupCPassword, signupUserName,
        recoverPassEmail, recoverPassUser, recoverUserEmail,
        emailOnLogin, emailOnSignUp;
    public Text notifTitle, notifMessage, profileUserNameText;
    public Toggle rememberMeLogin, rememberMeSignup, acceptPolicy;
    [SerializeField]
    private GameManager gameManager;
    private static string playerName;
    private static bool logged=false;

    public void Start()
    {
        if (logged==true)
        {
            if(SceneManager.GetActiveScene().name.Equals("UserInterface"))
            {
                OpenProfilePanel();
            }
            
            profileUserNameText.text = playerName;
            playerName = null;
        }
    }
    public void Update()
    {
        
    }

    #region Panels
    
    private void EmailFromLoginToSignUp(string email, bool x)
    {
        if (x == true)
        {
            emailOnSignUp.text = "" + email;
        }
        else
        {
            emailOnLogin.text = "" + email;
        }
    }
    private void ShowNotificationMessage(string title, string message)
    {
        notificationPanel.SetActive(true);
        notifTitle.text = "" + title;
        notifMessage.text = "" + message;
    }
    public void CloseNotificationPanel()
    {
        notificationPanel.SetActive(false);
    }
    public void OpenLoginPanel(bool fromProfile) 
    {
        loginPanel.SetActive(true);
        if (fromProfile==false) 
        {
            EmailFromLoginToSignUp(emailOnSignUp.text, false);
        }
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
        rememberMeLogin.enabled = false;
        rememberMeSignup.enabled = false;
    }

    public void OpenSignupPanel()
    {
        EmailFromLoginToSignUp(emailOnLogin.text, true);
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
        rememberMeLogin.enabled = false;
        rememberMeSignup.enabled = false;
    }
    public void OpenProfilePanel()
    {
        profilePanel.SetActive(true);
        signupPanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
        loginPanel.SetActive(false);
    }
    public void ReadPrivacyPolicy()
    {
        privacyPolicyPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        recoverUserPanel.SetActive(false);
    }
    public void OpenRecoverUserPanel()
    {
        recoverUserPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPolicyPanel.SetActive(false);
    }
    #endregion

    #region Logout, Login and Signup
    public void LogOut()
    {
        logged = false;
        profileUserNameText.text = "";
        //loginEmail.text = "";
        loginPassword.text = "";
        signupEmail.text = "";
        signupPassword.text = "";
        signupCPassword.text = "";
        signupUserName.text = "";
        playerName = null;
        OpenLoginPanel(true);
    }

    public void LoginUser() 
    {
        //fill the fields
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text)) 
        {
            ShowNotificationMessage("FIELDS EMPTY", "Please, introduce all details.");
            return;
        }
        //if password is less than 6 message = too short
        if (loginPassword.text.Length < 6)
        {
            ShowNotificationMessage("Sorry!", "Password too short!\nIntroduce at least 6 characters.");
            return;
        }

        //Do login
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text,

            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string displayName = null;
        if (result.InfoResultPayload is not null) 
        {
            displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        Debug.Log("OnLoginSuccess: "+displayName);
        StartCoroutine(LoadNext(displayName));
    }
    IEnumerator LoadNext(string displayName) 
    {
        Debug.Log("LoadNext: "+displayName);
        logged = true;
        playerName = displayName;
        profileUserNameText.text = displayName;
        ShowNotificationMessage("", "Welcome " + displayName + "!");
        yield return new WaitForSeconds(2);
        OpenProfilePanel();
        notificationPanel.SetActive(false);
    }

    public void SignUpUser() 
    {
        //username > 6
        if (signupUserName.text.Length < 6)
        {
            ShowNotificationMessage("Sorry!", "Username too short!\nIntroduce at least 6 characters.");
            return;
        }
        //username too long
        if (signupUserName.text.Length > 12)
        {
            ShowNotificationMessage("Sorry!", "Username too long!\nMax 12 characters.");
            return;
        }
        //fill the fields
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            ShowNotificationMessage("FIELDS EMPTY", "Please, introduce all details.");
            return;
        }

        //if password is less than 6 message = too short
        if (signupPassword.text.Length < 6) 
        {
            ShowNotificationMessage("Sorry!", "Password too short!\nIntroduce at least 6 characters.");
            return;
        }
        //confirm password
        if (!signupPassword.text.Equals(signupCPassword.text))
        {
            ShowNotificationMessage("Sorry!", "The password confirmation is not the same.");
            return;
        }
        //accept policy
        if (!acceptPolicy.isOn)
        {
            ShowNotificationMessage("Sorry!", "You need to accept our policy.");
            return;
        }

        //Do SignUp
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = signupUserName.text,
            Username = signupUserName.text,
            Email = signupEmail.text,
            Password = signupPassword.text,

            RequireBothUsernameAndEmail = true,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnError);
        
    }

    private void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        //ShowNotificationMessage("Welcome!", "Your account have been created successfuly.");
        StartCoroutine(LoadNext(signupUserName.text));
        //OpenProfilePanel();
    }
    #endregion

    private void OnError(PlayFabError error)
    {
        ShowNotificationMessage("Sorry!", error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }

    #region Recover Access
    public void RecoverUser()
    {
        if (string.IsNullOrEmpty(recoverUserEmail.text))
        {
            ShowNotificationMessage("EMAIL EMPTY","Please, introduce your email.");
            return;
        }
        //Recover user
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = recoverUserEmail.text,
            TitleId = "E46C1",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        OpenLoginPanel(false);
        ShowNotificationMessage("Success!","Recovery mail sent.");
    }
    #endregion

    #region Scenes
    public void playPVP()
    {
        //move the playerName to the GameManager object for have it on the next Game Scene
        //if (gameManager != null)
        //{
            gameManager.playerName = profileUserNameText.text;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
}
    public void openDB()
    {
        //if (gameManager != null)
        //{
            if (logged == true)
            {
                gameManager.playerName = profileUserNameText.text;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        //}
    }
    public void openUserInterface()
    {
        //if (gameManager != null)
        //{
        //as Logged
        if (logged == true)
        {
            gameManager.playerName = profileUserNameText.text;
            //SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex + 1);
        }

        //IF PETITION COMES FROM THE SAME SCENE, JUST OPEN THE LOGINPANEL or PROFILE IF ITS LOGGED
        if (SceneManager.GetActiveScene().name.Equals("UserInterface"))
        {
            if (logged == true)
            {
                OpenProfilePanel();
            }
            else
            {
                OpenLoginPanel(false);
            }
            profileUserNameText.text = playerName;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex + 1);
        }
        
        //as NO logged
        
        //OpenLoginPanel(false);
        //}
    }
    #endregion

}
