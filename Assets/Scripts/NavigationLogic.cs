using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class NavigationLogic : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] classPositions;

    public void Start()
    {
        Random random = new Random();
        int index = random.Next(0, classPositions.Length);
        navMeshAgent.SetDestination(classPositions[index].position);
    }
}
