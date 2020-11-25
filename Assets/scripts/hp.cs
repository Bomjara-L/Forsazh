using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public GameObject Car;
    public bool alive = true;
    void Update()
    {
        if (alive == false)
        {
            SceneManager.LoadScene(1);
        }
    }
}
