using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LoginManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private GameObject errorButton;
    [SerializeField] private GameObject successButton;
    [SerializeField] private GameObject loginPannel;
    [SerializeField] private GameObject messagePannel;
    [SerializeField] private TextMeshProUGUI messageText;

    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        PlaySound.instance.soundPlay();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Please fill in all fields.", errorButton);
            return;
        }

        if (!RegisterManager.IsValidInput(username) || !RegisterManager.IsValidInput(password))
        {
            ShowMessage("Username or Password must be in English or numbers.", errorButton);
            return;
        }

        StartCoroutine(LoginUser(username, password));
    }

    IEnumerator LoginUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/login_management/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                var response = JsonUtility.FromJson<Response>(jsonResponse);

                if (response.status == "success")
                {
                    ShowMessage(response.message, successButton);
                    PlayerPrefs.SetString("LoginMessage", "Login successful!");
                    PlayerPrefs.SetInt("UserID", response.id);
                    PlayerPrefs.SetInt("Diamonds", response.data.diamonds);
                    PlayerPrefs.SetInt("Hearts", response.data.hearts);
                    Debug.Log("USER ID: " + PlayerPrefs.GetInt("UserID"));
                    Debug.Log("Status: " + response.status);
                    Debug.Log("Message: " + response.message);
                }
                else
                {
                    ShowMessage(response.message, errorButton);
                    Debug.Log("Status: " + response.status);
                    Debug.Log("Message: " + response.message);

                }
            }
            else
            {
                ShowMessage("Request error: " + www.error, errorButton);
            }
        }
    }

        void ShowMessage(string message, GameObject buttonName)
    {
        if (messageText != null)
        {
            loginPannel.SetActive(false);
            messagePannel.SetActive(true);
            buttonName.gameObject.SetActive(true);
            messageText.text = message;
        }
    }

    public static bool IsEnglish(string username)
    {
        string pattern = @"^[a-zA-Z\s]+$";
        return Regex.IsMatch(username, pattern);
    }

    public static bool IsValidPassword(string password)
    {
        bool hasLetter = Regex.IsMatch(password, @"[a-zA-Z]");
        bool hasDigit = Regex.IsMatch(password, @"\d");
        bool hasSpecialChar = Regex.IsMatch(password, @"[\W_]");
        return hasLetter && hasDigit && hasSpecialChar;
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
        public int id;
        public UserData data;

        [System.Serializable]
        public class UserData
        {
            public int diamonds;
            public int hearts;
        }
    }
}
