using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void EndGame()
    {
        Application.Quit();
    }

    public void ShowUI()
    {
        GameObject.Find("Canvas/Mask/UI").SetActive(true);
    }
}
