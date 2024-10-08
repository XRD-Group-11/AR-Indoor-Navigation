using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCameraCalibrationWithWorldPosition : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;
    public XROrigin arSessionOrigin; 


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

    // Calibrate the camera position and rotation based on the QR code
    private void CalibrateCamera(ARTrackedImage trackedImage)
    {
        // Get the position and rotation of the detected QR code in AR space
        Vector3 detectedQrPosition = trackedImage.transform.position;
        Quaternion detectedQrRotation = trackedImage.transform.rotation;

        // Calculate the position and rotation offsets
        Vector3 positionOffset = qrCodeRealWorldPosition - detectedQrPosition;
        Quaternion rotationOffset = qrCodeRealWorldRotation * Quaternion.Inverse(detectedQrRotation);

        // Smoothing factors to reduce jitter (adjust values based on your preference)
        float positionSmoothingFactor = 0.1f;
        float rotationSmoothingFactor = 0.1f;

        // Position threshold to determine if the movement is significant
        float positionThreshold = 0.01f;  // Minimum movement in meters to trigger updates
        float rotationThreshold = 1f;     // Minimum rotation change in degrees to trigger updates

        // Apply position and rotation updates only if they exceed the defined threshold
        if (Vector3.Distance(arSessionOrigin.transform.position, positionOffset) > positionThreshold ||
            Quaternion.Angle(arSessionOrigin.transform.rotation, rotationOffset * arSessionOrigin.transform.rotation) > rotationThreshold)
        {
            // Smoothly interpolate the camera's position
            arSessionOrigin.transform.position = Vector3.Lerp(arSessionOrigin.transform.position, positionOffset, positionSmoothingFactor);

            // Smoothly interpolate the camera's rotation
            arSessionOrigin.transform.rotation = Quaternion.Slerp(arSessionOrigin.transform.rotation, rotationOffset * arSessionOrigin.transform.rotation, rotationSmoothingFactor);
        }

        Debug.Log("Calibrated Position: " + arSessionOrigin.transform.position);
    } 
}