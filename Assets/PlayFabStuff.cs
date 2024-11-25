using PlayFab.ClientModels;
using PlayFab;
using System.Diagnostics;
using UnityEngine;
public interface ILogin
{
    void Login(System.Action<LoginResult> onSuccess, System.Action<PlayFabError> onFailure);
}

public class DeviceLogin : ILogin
{
    private string deviceID;
    public DeviceLogin(string deviceID)
    {
        this.deviceID = deviceID;
    }
    public void Login(System.Action<LoginResult> onSuccess, System.Action<PlayFabError> onFailure)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onFailure);
    }
}

public class LoginManager
{
    private ILogin loginMethod;
    public void SetLoginMethod(ILogin loginMethod)
    {
        this.loginMethod = loginMethod;
    }
    public void Login(System.Action<LoginResult> onSuccess, System.Action<PlayFabError> onFailure)
    {
        if (loginMethod != null)
        {
            loginMethod.Login(onSuccess, onFailure);
        } else
        {
            UnityEngine.Debug.LogError("No Login method set!");
        }
    }
}

public class PlayFabManager
{
    private LoginManager loginManager;
    private string savedEmailKey = "SavedEmail";
    private string userEmail;
    private void Start()
    {
        loginManager = new LoginManager();
        if (PlayerPrefs.HasKey(savedEmailKey)){
            string savedEmail = PlayerPrefs.GetString(savedEmailKey);
            EmailLoginButtonClicked(savedEmail, "SavedPassword");
        }
    }
    public void EmailLoginButtonClicked(string email, string password)
    {
        userEmail = email;
        //loginManager.SetLoginMethod(new EmailLogin(email, password));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        if(!string.IsNullOrEmpty(userEmail))
            PlayerPrefs.SetString(savedEmailKey, userEmail);
        LoadPlayerData(result.PlayFabId);
    }
    private void OnLoginFailure(PlayFabError error)
    {
        UnityEngine.Debug.LogError("Login failed: " + error.ErrorMessage);
    }
    private void LoadPlayerData(string playFabId)
    {
        var request = new GetUserDataRequest { PlayFabId = playFabId };
        //PlayFabClientAPI.GetUserData(request, OnDataSuccess, OnDataFailure);
    }
    private void OnDataSuccess(GetUserDataRequest result)
    {
        UnityEngine.Debug.Log("Player data loaded successfully");
    }
    private void OnDataFailure(PlayFabError error)
    {
        UnityEngine.Debug.Log("Player data loaded successfully");
    }
}