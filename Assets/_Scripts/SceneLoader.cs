using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Hub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
