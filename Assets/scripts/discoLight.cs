using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discoLight : MonoBehaviour
{
    public Light svet; 
    public float time1;
    public float time2;
    public float time3;
    public float red;
    public float green;
    public float blu;
    
    void Start()
    {
        time1 = 0.5f;
        time2 = 1f;
        time3 = 1.5f;
        red = svet.color.r;
        green = svet.color.g;
        blu = svet.color.b;
    }

    
    void Update()
    {
        time1 -= Time.deltaTime;
        time2 -= Time.deltaTime;
        time3 -= Time.deltaTime;
        

        if (time1 <= 0)
        {
            red = 255f;
            green = 0f;
            blu = 255f;
            time1 = 0.4f;
            svet.color = new Color(red, green, blu);
        }

        if (time2 <= 0)
        {
            red = 0f;
            green = 255f;
            blu = 0f;
            time2 = 0.5f;
            svet.color = new Color(red, green, blu);
        }

        if (time3 <= 0)
        {
            red = 255f;
            green = 0f;
            blu = 0f;
            time3 = 0.6f;
            svet.color = new Color(red, green, blu);
        }
        
    }
} 
