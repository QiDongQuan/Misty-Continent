using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillCanLearn : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject confirm;
    PlayerCharacter player;
    JsonData data;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

        string str = Resources.Load<TextAsset>("JsonData/BuffData").text;
        data = JsonMapper.ToObject(str);

        foreach (var key in data.Keys)
        {
            BuffData buffData = Resources.Load<BuffData>(data[key]["Path"].ToString());
            if (!buffData.isSkill)
            {
                continue;
            }   
            GameObject buttonObj = Instantiate(buttonPrefab, transform);
            buttonObj.name = $"Button_{key}";

            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null) buttonText.text = buffData.buffName;
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(delegate { ButtonClicked(key); });
            }
            foreach (BuffInfo info in player.GetComponent<BuffHandler>().buffList)
            {
                if(info.buffData.id == buffData.id)
                {
                    buttonText.text = buffData.buffName + "(ÒÑÑ§Ï°)";
                    button.onClick.RemoveAllListeners();
                }
            } 
        }
    }

    public void ButtonClicked(string key)
    {
        confirm.SetActive(true);
        Button button = confirm.transform.Find("Confirm").GetComponent<Button>();
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { player.AddBuff(data[key]["Path"].ToString(),player.gameObject,player.gameObject); });
            button.onClick.AddListener(delegate {
                confirm.SetActive(false);
            });
        }
    }
}
