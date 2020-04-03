using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loyRide : MonoBehaviour
{
    
    public GameObject car;
    public Quaternion rot;
    public float time = 10f;
   
    void Start()
    {
        car.transform.rotation = rot;
    }

    
    void Update()
    {
        time -= Time.deltaTime;
        if (time <=0)
        {
            rot.x = -8;
            time = 0.8f;
            car.transform.rotation = rot;
        }
       
    }
}
