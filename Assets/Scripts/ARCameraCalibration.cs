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
        Vector3 detectedQrPosition = trackedImage.transform.position;
        Quaternion detectedQrRotation = trackedImage.transform.rotation;

        Vector3 positionOffset = qrCodeRealWorldPosition - detectedQrPosition;
        Quaternion rotationOffset = qrCodeRealWorldRotation * Quaternion.Inverse(detectedQrRotation);

        arSessionOrigin.transform.position += positionOffset;
        arSessionOrigin.transform.rotation = rotationOffset * arSessionOrigin.transform.rotation;

        Debug.Log(detectedQrPosition);
    }
}