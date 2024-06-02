using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterMap : MonoBehaviour
{

    public GameObject buttonPrefab;

    void Start()
    {
        string str = Resources.Load<TextAsset>("JsonData/MapEnemyData").text;
        JsonData data = JsonMapper.ToObject(str);

        foreach (var key in data.Keys)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, transform);
            buttonObj.name = $"Button_{key}";

            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null) buttonText.text = key;

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(delegate { ButtonClicked(key); });
            }
        }
    }

    public void ButtonClicked(string key)
    {
        PlayerPrefs.SetInt("MapId", int.Parse(key));
        PlayerPrefs.SetString("LoadSceneName","Game01");
        SceneManager.LoadScene("Loading");
    }
}
