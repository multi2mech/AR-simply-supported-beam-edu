using UnityEngine;

public class FaceUser : MonoBehaviour
{
    public Transform cameraTransform; // Assign the headset camera (OVRCameraRig or XR camera)
    public float distanceFromUser = 1.0f; // Distance in front of the user
    public bool rotateToFaceUser = true; // Whether to rotate the object to always face the user

    void Update()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform; // Find the main camera if not assigned
        }

    if (cameraTransform != null)
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceFromUser;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f); // Smooth movement

        if (rotateToFaceUser)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }
    }
    }
}