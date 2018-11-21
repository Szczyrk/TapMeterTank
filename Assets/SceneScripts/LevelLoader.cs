using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadLevelName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(int sceneIndex)
    {
        //StartCoroutine(LoadAsynchronously(sceneIndex));
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);


        while(!operation.isDone)
        {


            yield return null;
        }
    }
}
