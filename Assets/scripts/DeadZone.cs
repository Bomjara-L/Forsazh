using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Transform Car;
    public Transform Spawn;

    private void OnCollisionEnter(Collision collision)
    {
        Car.position = Spawn.position;
        Car.rotation = Spawn.rotation;
    }

}
