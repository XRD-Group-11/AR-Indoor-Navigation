using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCameraCalibrationWithWorldPosition : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;
    public XROrigin arSessionOrigin; 
    public Camera arCamera;


    public Vector3 qrCodeRealWorldPosition = new Vector3(0, 0, 0); // In meters
    public Quaternion qrCodeRealWorldRotation = Quaternion.Euler(0, 0, 0); // In degrees

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.enabled = true;
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        Debug.Log("AR Camera Calibration mounted");
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            CalibrateCamera(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                CalibrateCamera(trackedImage);
            }
        }
    }

    private void CalibrateCamera(ARTrackedImage trackedImage)
    {
        Vector3 detectedQrPosition = trackedImage.transform.position;
        Quaternion detectedQrRotation = trackedImage.transform.rotation;
        
        Vector3 cameraPosition = arCamera.transform.position;
        Quaternion cameraRotation = arCamera.transform.rotation;
        
        Vector3 relativePosition = detectedQrPosition - cameraPosition;
        Quaternion relativeRotation = detectedQrRotation * Quaternion.Inverse(cameraRotation);


        Vector3 positionOffset = qrCodeRealWorldPosition - relativePosition - arSessionOrigin.Camera.transform.localPosition;
        Quaternion rotationOffset = qrCodeRealWorldRotation * relativeRotation;

        // Smoothing factors to reduce jitter (adjust values based on your preference)
        float positionSmoothingFactor = 0.25f;

        float positionThreshold = 0.01f;  // Minimum movement in meters to trigger updates

        if (Vector3.Distance(arSessionOrigin.transform.position, positionOffset) > positionThreshold)
        {
            arSessionOrigin.transform.position = Vector3.Lerp(arSessionOrigin.transform.position, positionOffset, positionSmoothingFactor);
        }
            arSessionOrigin.Camera.transform.LookAt(qrCodeRealWorldPosition);
    } 
}