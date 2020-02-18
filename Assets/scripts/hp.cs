using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hp : MonoBehaviour
{
    public GameObject Car;
    public int healthPoints = 3;

    void Update()
    {

        if (healthPoints <= 0)
        {
            Destroy(Car);
            Debug.Log("машина уничтожена");
            SceneManager.LoadScene(1);
        }

    }
}
