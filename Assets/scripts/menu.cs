using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class menu : MonoBehaviour
{

    public Text explame;
    public GameObject button;
    public Dropdown DropDown;
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

    public void Settings()
    {
        SceneManager.LoadScene(8);

    }
    public void DropD()
    {

        Resolution [] resolution = Screen.resolutions;
        Resolution [] res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for (int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
        }
        DropDown.ClearOptions();
        DropDown.AddOptions(strRes.ToList());
        DropDown.value = res.Length - 1;
        Screen.SetResolution(res[res.Length - 1].width, res[res.Length - 1].height, true);
    }

    public void back()
    {
        SceneManager.LoadScene(0);

    }

    }
    

