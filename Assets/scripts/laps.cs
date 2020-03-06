using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laps : MonoBehaviour
{
    public int no_checks;
    public int curr_check;
    public int no_laps;
    public int curr_lap;
    private void Start()
    {
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
    }

    private void OnTriggerEnter(Collider check_col)
    {
        if (check_col.name == curr_check.ToString())
        {
            curr_check++;
        }
    }

}
