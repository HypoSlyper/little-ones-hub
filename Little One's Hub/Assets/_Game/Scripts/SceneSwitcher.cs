using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    string sceneNameTemp;

    public void SwitchScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNameTemp);
    }

    public void SetScene(string sceneName)
    {
        sceneNameTemp = sceneName;
    }
}
