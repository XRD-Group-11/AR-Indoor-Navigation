using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navMeshAgent;

    void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
