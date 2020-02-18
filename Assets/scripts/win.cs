using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    public GameObject Ffinish;
    private Finish finish;
    public GameObject winTriger;
    void Start()
    {
        finish = Ffinish.GetComponent<Finish>();
    }

    void Update()
    {
        if (finish.win == 1)
        {
            Instantiate(winTriger, transform.position, transform.rotation);
            finish.win++;
        }
    }
}
