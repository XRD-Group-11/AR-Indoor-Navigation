using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
        // Update is called once per frame
        void Start()
        {
        }
    void Update()
    {
        navMeshAgent.updatePosition = false;
        navMeshAgent.nextPosition = target.position;
    }
}
