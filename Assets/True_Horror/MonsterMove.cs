using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent enemy;
    public GameObject Player;
    void Update()
    {
        enemy.destination = player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {   
            Debug.Log("монстр скушал машину");
            Player.GetComponent<Hp>().alive = false;
        }
    }
}
