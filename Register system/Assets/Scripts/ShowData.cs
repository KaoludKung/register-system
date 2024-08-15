using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShowData : MonoBehaviour
{
    [SerializeField] private Image heartSlider;
    [SerializeField] private TextMeshProUGUI diamondText;
    private const int condition = 10;

    void Start()
    {
        if (PlayerPrefs.HasKey("LoginMessage"))
        {
            LoadAndSetData();
            string message = PlayerPrefs.GetString("LoginMessage");
            Debug.Log(message);
            PlayerPrefs.DeleteKey("LoginMessage");
        }
    }

    void Update()
    {
        LoadAndSetData();
    }

    void LoadAndSetData()
    {
        int diamonds = PlayerPrefs.GetInt("Diamonds");
        int hearts = PlayerPrefs.GetInt("Hearts");

        if (hearts > 0)
        {
            heartSlider.fillAmount = Mathf.Clamp01((float)hearts / condition);
        }
        else
        {
            heartSlider.fillAmount = 0;
        }

        diamondText.text = diamonds.ToString();

    }

}
