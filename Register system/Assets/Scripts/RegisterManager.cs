using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;
    [SerializeField] private GameObject errorButton;
    [SerializeField] private GameObject successButton;
    [SerializeField] private GameObject signupPannel;
    [SerializeField] private GameObject messagePannel;
    [SerializeField] private TextMeshProUGUI messageText;
    

    public void OnRegisterButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        PlaySound.instance.soundPlay();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ShowMessage("Please fill in all fields.",errorButton);
            return;
        }

        if (password != confirmPassword)
        {
            ShowMessage("Password do not match.",errorButton);
            return;
        }

        if (!IsValidInput(username) || !IsValidInput(password))
        {
            ShowMessage("Username or Password must be in English or numbers.",errorButton);
            return;
        }

        StartCoroutine(RegisterUser(username, password));

    }

    IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("confirm_password", password);
        form.AddField("diamonds", 0);
        form.AddField("hearts", 10);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/login_management/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                string jsonResponse = www.downloadHandler.text;
                var response = JsonUtility.FromJson<Response>(jsonResponse);

                if (response.status == "success")
                {
                    ShowMessage(response.message, successButton);
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
                ShowMessage("Request error: " + www.error,errorButton);
            }
        }
    }

    void ShowMessage(string message,GameObject buttonName)
    {
        if (messageText != null)
        {
            signupPannel.SetActive(false);
            messagePannel.SetActive(true);
            buttonName.gameObject.SetActive(true);
            messageText.text = message;
        }
    }

    public static bool IsValidInput(string input)
    {
        bool hasLetter = Regex.IsMatch(input, @"[a-zA-Z]");
        bool hasDigit = Regex.IsMatch(input, @"\d");

        return hasLetter || hasDigit;
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
    }
}
