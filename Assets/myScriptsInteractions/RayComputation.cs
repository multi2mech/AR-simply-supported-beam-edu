using UnityEngine;

public class RayComputation : MonoBehaviour
{
    public float InitialLength { get; private set; } // Initial ray length when selected
    private ControllerTriggerManager triggerManager; // Reference to the ControllerTriggerManager

    public Vector3 RayOrigin { get; private set; } // Start of the ray (controller position)
    public Vector3 RayDirection { get; private set; } // Direction of the ray
    public Vector3 RayEnd { get; private set; } // End of the ray (object position)
    public float RayLength { get; private set; } // Length of the ray
    public bool debugRayQ = false; // Debug Ray
    public bool debugRayOnlyForDirectionQ = false; // Debug Ray
    public bool IsRayValid { get; private set; } = false; // Whether the ray is valid

    public Quaternion RayRotation { get; private set; } // Rotation of the ray
    public Vector3 RayDistance { get; private set; } // Length of the ray
    public Transform controllerTransform { get; private set; } // Reference to the controller transform

    public Quaternion InitialControllerRotation { get; private set; } // Initial rotation offset as quaternion
    private string controllerRorL = "RightHandAnchor";
    public Quaternion calibrationOffset; // Calibration offset as quaternion

    void Start()
    {   
        GameObject globalAux = GameObject.Find("GlobalAux");
        if (globalAux != null)
        {
            // Get the ControllerTriggerManager component
            triggerManager = globalAux.GetComponent<ControllerTriggerManager>();

            if (triggerManager != null)
            {
                Debug.Log("ControllerTriggerManager found on GlobalAux.");
            }
            else
            {
                Debug.LogError("ControllerTriggerManager component not found on GlobalAux.");
            }
        }
        else
        {
            Debug.LogError("GlobalAux object not found in the scene.");
        }
        ComputeRay();
        // Compute the initial ray offset
        calibrationOffset = ComputeCalibrationOffset(RayRotation);
        
    }

    void Update()
    {
        // Update the ray dynamically each frame
        ControllerCheck();
        ComputeRay();

        // Log the ray's data for debugging
        LogRayInfo();
    }

    private void ComputeRay()
    {
        // Get the controller or hand's transform
        controllerTransform = GetControllerTransform();

        if (controllerTransform != null)
        {
            // Use the controller's position as the ray origin
            RayOrigin = controllerTransform.position;
            RayRotation = controllerTransform.rotation;
            // The object's position is the ray end
            RayEnd = transform.position;
            // Compute the ray's direction and length
            RayDirection = controllerTransform.forward;
            RayDistance = RayEnd - RayOrigin;
            RayLength = Vector3.Distance(RayOrigin, RayEnd);
            IsRayValid = true;
            // Optional: Visualize the ray
            // Debug.DrawRay(RayOrigin, RayDirection * RayLength, Color.green);
        }
        else
        {
            IsRayValid = false;
        }
    }

    // public void ComputeInitialOffset()
    // {   if (controllerTransform != null)
    //     {
    //         InitialControllerRotation = controllerTransform.rotation;
    //     }
    //     // Debugging
    //     Debug.Log($"[RayComputation] Initial Controller Rotation (Quaternion): {InitialControllerRotation}");
    //     Debug.Log($"[RayComputation] Initial Controller Rotation (Euler): {InitialControllerRotation.eulerAngles}");
    // }

    private void LogRayInfo()
    {
        if (IsRayValid)
        {
            // Compute the ray's angle (relative to world forward)
            float rayAngle = Vector3.Angle(Vector3.forward, RayDirection);
            if (debugRayQ)
            {
                Debug.Log($"[RayComputation] Ray Start (Controller): {RayOrigin}");
                Debug.Log($"[RayComputation] Ray End (Object): {RayEnd}");
                Debug.Log($"[RayComputation] Ray Direction: {RayDirection}");
                Debug.Log($"[RayComputation] Ray Length: {RayLength}");
                Debug.Log($"[RayComputation] Ray Angle: {rayAngle}°");
            }
            if (debugRayOnlyForDirectionQ)
            {
                Debug.Log($"[RayComputation] Ray Direction: {RayDirection}");
            }
        }
        else
        {
            Debug.Log("[RayComputation] Ray is invalid.");
        }
    }

public void ControllerCheck()
    {
        if (triggerManager != null)
        {
            // Call the IsRightControllerActive() method
            bool isRightActive = triggerManager.IsRightControllerActive();

            if (isRightActive)
            {
                //controllerRorL = "RightHandAnchor";
                controllerRorL = "RightHandAnchor";
            }
            else
            {
                controllerRorL = "LeftHandAnchor";
            }
        }
        
        
    }

    public Transform GetControllerTransform()
    {
        // Replace with actual code to fetch the controller's transform (e.g., right hand)
        GameObject controller = GameObject.Find(controllerRorL); // Adjust this to your setup
        if (controller != null)
        {
            return controller.transform; // Return the transform for global position and orientation
        }

        Debug.LogWarning("[GetControllerTransform] Controller transform not found.");
        return null; // Controller not found
    }


private Quaternion ComputeCalibrationOffset(Quaternion initialControllerRotation)
{
    // Define the desired forward and up vectors
    Vector3 desiredForward = Vector3.forward; // Global Z-axis
    Vector3 desiredUp = Vector3.up;           // Global Y-axis

    // Get the controller's actual forward and up vectors
    Vector3 actualForward = initialControllerRotation * Vector3.forward;
    Vector3 actualUp = initialControllerRotation * Vector3.up;
    //Debug.Log($"Actual Forward: {actualForward}");
    //Debug.Log($"Actual Up: {actualUp}");
    // Compute the rotation to align the controller's forward vector to the desired forward
    Quaternion forwardAlignment = Quaternion.FromToRotation(actualForward, desiredForward);

    // Use the forward alignment to align the up vector as well
    Vector3 adjustedUp = forwardAlignment * actualUp;
    Quaternion upAlignment = Quaternion.FromToRotation(adjustedUp, desiredUp);

    // Combine the two alignments to form the full calibration offset
    calibrationOffset = upAlignment * forwardAlignment;

   // Debug.Log($"Computed Calibration Offset (Quaternion): {calibrationOffset}");
    //Debug.Log($"Computed Calibration Offset (Euler): {calibrationOffset.eulerAngles}");

    return calibrationOffset;
}


}


