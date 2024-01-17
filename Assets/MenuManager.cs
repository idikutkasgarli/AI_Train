using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Input.ResetInputAxes();

    }
    public void PlayGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Quit!");
        Application.Quit();
    }

    public void ReturnMenu()
    {
        UnityEngine.Debug.Log("Menu!");
        SceneManager.LoadScene(0);
    }
}
