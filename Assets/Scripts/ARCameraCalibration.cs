using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCameraCalibrationWithWorldPosition : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;
    private Camera _arCamera;

    // Define the real-world position and rotation of the QR code
    public Vector3 qrCodeRealWorldPosition = new Vector3(0, 0, 0); // In meters
    public Quaternion qrCodeRealWorldRotation = Quaternion.Euler(0, 0, 0); // In degrees

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _arCamera = Camera.main;
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        Debug.Log("AR Camera Calibration mounted");
    }

    // Handle the tracked images change event
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log($"QR Code Detected: {trackedImage.referenceImage.name}");
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            Debug.Log($"QR Code Updated: {trackedImage.referenceImage.name} - Tracking State: {trackedImage.trackingState}");
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log($"QR Code Removed: {trackedImage.referenceImage.name}");
        }
        // Handle newly detected images
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            CalibrateCamera(trackedImage);
        }

        // Optionally handle updated images
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
        // Get the position and rotation of the QR code as detected by ARCore
        Vector3 detectedQrPosition = trackedImage.transform.position;
        Quaternion detectedQrRotation = trackedImage.transform.rotation;

        // Calculate the relative difference between the real-world position and detected position
        Vector3 positionOffset = qrCodeRealWorldPosition - detectedQrPosition;

        // Calculate the rotational difference
        Quaternion rotationOffset = qrCodeRealWorldRotation * Quaternion.Inverse(detectedQrRotation);

        // Apply the offset to the AR Camera's position and rotation
        _arCamera.transform.position += positionOffset;
        _arCamera.transform.rotation = rotationOffset * _arCamera.transform.rotation;

        Debug.Log("AR Camera has been calibrated using the QR code!");
    }
}