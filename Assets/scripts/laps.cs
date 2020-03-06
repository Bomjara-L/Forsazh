using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class laps : MonoBehaviour
{
    public int no_checks;
    public int curr_check;
    public int no_laps;
    public int curr_lap;
    public int lap_count;
    private void Start()
    {
        lap_count = 3;
        no_checks = GameObject.Find("Checkpoints").transform.childCount;
        curr_check = 1;
        no_laps = 3;
        curr_lap = 1;
    }


    private void Update()
    {
        if (curr_check > no_checks)
        {
            curr_lap++;
            curr_check = 1;
        }

        if (lap_count < curr_lap)
        {
            SceneManager.LoadScene(3);
        }
    }

    private void OnTriggerEnter(Collider check_col)
    {
        if (check_col.name == curr_check.ToString())
        {
            curr_check++;
        }
    }

}
