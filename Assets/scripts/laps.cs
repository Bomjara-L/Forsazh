using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class laps : MonoBehaviour
{
    public int no_checks;
    public int curr_check;
    public int no_laps;
    public int curr_lap;
    public int lap_count;
    public Text curr_check_text;
    public Text curr_lap_text;
    private Transform points_cont;
    private void Start()
    {
        
        lap_count = 3;
        points_cont = GameObject.Find("Checkpoints").transform;
        no_checks = points_cont.childCount;
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
            for (int i = 0; i < no_checks; i++)
            {
                points_cont.GetChild(i).gameObject.SetActive(true);
            } 
        }

        if (lap_count < curr_lap)
        {
            if (no_checks < 13)
            {
                SceneManager.LoadScene(3);
            }
            else SceneManager.LoadScene(4);

        }      

        curr_lap_text.text = curr_lap.ToString();
        
        curr_check_text.text = curr_check.ToString();
    

}

    void OnTriggerEnter(Collider check_col)
    {
        
        if (check_col.name == curr_check.ToString())
        {
            curr_check++;
            check_col.gameObject.SetActive(false);
        }
    }

}
