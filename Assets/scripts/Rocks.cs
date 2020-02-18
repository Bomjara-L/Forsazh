using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    public int damage;
    private hp hp;
    public GameObject SportCar;
    public GameObject rock;

    private void Awake()
    {
        hp = SportCar.GetComponent<hp>();
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(rock);
        hp.healthPoints--;
        Debug.Log("наезд на камень");

    }
}
