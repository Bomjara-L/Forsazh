using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Respawn : MonoBehaviour
{
    public Transform Car;
    public Transform Spawn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Car.position = Spawn.position;
            Car.rotation = Spawn.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }
}