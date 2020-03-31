using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene(2);
    }

    public void multiplayer()
    {
        SceneManager.LoadScene(5);
    }

    public void exit()
    {
        Application.Quit();
    }

}
