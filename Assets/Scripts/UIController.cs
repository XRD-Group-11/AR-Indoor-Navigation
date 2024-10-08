using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Canvas canvas;

    public TargetIndicator targetIndicator;

    public Camera MainCamera;

    public GameObject TargetIndicatorPrefab;

    // Start is called before the first frame update
    void Start()
    {
         targetIndicator = GameObject.Instantiate(TargetIndicatorPrefab, canvas.transform).GetComponent<TargetIndicator>();
        targetIndicator.InitialiseTargetIndicator(MainCamera, canvas);
    }

    // Update is called once per frame
    void Update()
    {
        targetIndicator.UpdateTargetIndicator();
    }

    public void updateTargetIndicatorPosition(Vector3 target)
    {
        targetIndicator.UpdateTargetPosition(target);
    }

}
