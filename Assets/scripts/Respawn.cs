using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Respawn : MonoBehaviour
{
    public Rigidbody Car;
    public Transform Spawn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Car.position = Spawn.position;
            Car.rotation = Spawn.rotation;
            Car.velocity = Vector3.zero;
            
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.timeScale != .2f)
                Time.timeScale = .2f;
            else
                Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }
}