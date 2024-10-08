using UnityEngine;
using UnityEngine.AI;

public class NavigationRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public NavMeshAgent navMeshAgent;
    public float yOffset;
    private UIController _uiController;
    
    void Start()
    {
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;
        
        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    void LateUpdate()
    {
        if (navMeshAgent.hasPath)
        {
          DrawPath();
        }
    }

    void DrawPath()
    {
        int navCorners = navMeshAgent.path.corners.Length;
        lineRenderer.positionCount = navCorners;
        lineRenderer.SetPosition(0, transform.position);

        if (navCorners < 2)
        {
            return;
        }

        for (int i = 0; i < navCorners; i++)
        {
            Vector3 pointPosition = new Vector3(navMeshAgent.path.corners[i].x, navMeshAgent.path.corners[i].y + yOffset,
                navMeshAgent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }

        if (lineRenderer.positionCount > 1)
        {
            _uiController.updateTargetIndicatorPosition(lineRenderer.GetPosition(1));
        }
    }
}
