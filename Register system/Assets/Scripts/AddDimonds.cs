using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddDimonds : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diamondText;
    private int userID;
    private const int diamondAmount = 100;

    void Start()
    {
        if (PlayerPrefs.HasKey("UserID"))
        {
            userID = PlayerPrefs.GetInt("UserID", 0);
        }
        else
        {
            Debug.LogError("UserID not found in PlayerPrefs.");
        }
    }

    public void OnAddDiamondsButtonClicked()
    {
        StartCoroutine(addDiamonds(userID, diamondAmount));
    }

    IEnumerator addDiamonds(int userID, int diamond)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", userID);
        form.AddField("diamonds", diamond);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/login_management/addDiamond.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                var response = JsonUtility.FromJson<Response>(jsonResponse);

                if (response.status == "success")
                {
                    Debug.Log(response.message);
                    PlayerPrefs.SetInt("Diamonds", response.data.diamonds);
                    diamondText.text = response.data.diamonds.ToString();
                }
                else
                {
                    Debug.LogError("Failed to add diamonds: " + response.message);
                }
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
        public UserData data;

        [System.Serializable]
        public class UserData
        {
            public int diamonds;
        }
    }
}
