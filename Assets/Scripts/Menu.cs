using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Slider loading;
    [SerializeField] private GameObject loadingBlur;
    [SerializeField] private Text percent;
    
    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(Loading());
        
    }

    IEnumerator Loading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            loadingBlur.SetActive(true);
            float progress = Mathf.Clamp01(operation.progress / .9f);
           
            
            loading.value = progress;
            percent.text = progress * 100f + " %";
            yield return null;
        }
    }
}