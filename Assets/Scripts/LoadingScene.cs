using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingSlider;
    private AsyncOperation async;
    private int processNow;
    string SceneName;

    private void Start()
    {
        loadingSlider = GameObject.Find("SliderLoading").GetComponent<Slider>();
        processNow = 0;
        SceneName = PlayerPrefs.GetString("LoadSceneName");
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        if (async == null)
        {
            return;
        }

        int process;

        if (async.progress < 0.9f)
        {
            process = (int)(async.progress * 100);
        }
        else
        {
            process = 100;
        }

        if (processNow < process)
        {
            processNow++;
        }

        loadingSlider.value = processNow;

        if (processNow == 100)
        {
            async.allowSceneActivation = true;
        }
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        yield return async;
    }
}
