using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public Text explame;
    public GameObject button;
    public void play()
    {
        SceneManager.LoadScene(5);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void playGame()
    {
        SceneManager.LoadScene(2);
    }

    public void killExplame()
    {
        Destroy(explame);
        Destroy(button);
    }

    public void Multiplayer()
    {
        SceneManager.LoadScene(7);
    }
}
