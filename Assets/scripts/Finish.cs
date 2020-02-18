using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    public int circle;
    public int win = 0;

    private void OnTriggerExit(Collider other)
    {
            circle++;
            Debug.Log(circle);
    }

    void Update()
    {
        if (circle == 3)
        {
            Debug.Log("победа");
            SceneManager.LoadScene(3);
        }
    }
}
