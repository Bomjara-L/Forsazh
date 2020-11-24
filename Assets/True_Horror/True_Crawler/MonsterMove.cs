using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent enemy;
    void Update()
    {
        enemy.destination = player.position;
    }
}
