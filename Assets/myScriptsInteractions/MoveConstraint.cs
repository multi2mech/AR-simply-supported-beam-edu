using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Surfaces;
using System;

public class ConstraintMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement
    public float epsilon = 0.05f;
    public float sensitivityRatio = 0.05f;
    public float beamLength = 1f;  
    public float movingStep = 0.0001f; // Step of movement by controller
    private bool isSnapped = false; // Whether the object is snapped to the ray
    private RayComputation rayComputation; // Reference to the RayComputation script
    private SelectionManager selectionManager; // Reference to the SelectionManager script
    private Transform controllerTransform; // Reference to the controller's transform
    public bool debugMovementQ = false;

    private bool isMovalbeQ = false;

    private float elapsedTime1 = 0f; // Time elapsed since movement started
    private float elapsedTime2 = 0f; // Time elapsed since movement started

    private Quaternion currentRotation;
    private Quaternion initialRotation;
    private float increment = 1f;

    private Vector3 beamDirection = new Vector3(0,0,1);

    private Vector3 basePoint;
    private Vector3 endPoint;

    public void SetEndPoint(Vector3 point){
        endPoint = point;
    }
    public void SetBasePoint(Vector3 point){
        basePoint = point;
    }
    public void SetBeamDirection(Vector3 direction){
        beamDirection = direction;
    }
    public void SetBeamLength(float length){
        beamLength = length;
        UpdateEpsilon(beamLength * sensitivityRatio);
    }

    public void UpdateEpsilon(float newEpsilon){
        epsilon = newEpsilon;
    }

    public void SetMovable(bool isMovable){
        isMovalbeQ = isMovable;
    }

    public bool isMovableQ(){
        return isMovalbeQ;
    }

    void Start()
    {
        
        // Find the RayComputation component on this GameObject
        rayComputation = GetComponent<RayComputation>();
        if (rayComputation == null)
        {
            Debug.LogError("[ObjectMovement] RayComputation not found on " + gameObject.name);
        }

        // Find the SelectionManager component on this GameObject
        selectionManager = GetComponent<SelectionManager>();

        if (selectionManager == null)
        {
            Debug.LogError("[ObjectMovement] SelectionManager not found on " + gameObject.name);
        }
        
    }

    void Update()
    {
        // Ensure the object is selected before proceeding
        if (isMovalbeQ) {
        if (selectionManager != null && selectionManager.isSelected)
        {
            //Debug.Log("SELECTED");
            // Proceed only if the ray is valid
            if (rayComputation != null && rayComputation.IsRayValid)
            
                //Debug.Log("COMPUTED");
                // Snap the object to the ray on the first selection
                if (!isSnapped)
                {
                }

                Quaternion calibrationOffset =  rayComputation.calibrationOffset;
                Quaternion initialRotation =  selectionManager.initialControllerRotation;
                controllerTransform = rayComputation.controllerTransform;
                Quaternion currentRotation =  calibrationOffset * controllerTransform.rotation; 
                Vector3 sourceVector = initialRotation * Vector3.forward;
                Vector3 targetVector = currentRotation * Vector3.forward;
                Quaternion rotation = Quaternion.FromToRotation(sourceVector, targetVector);

                Quaternion relativeRotation = currentRotation * Quaternion.Inverse(initialRotation) ;
                
                Vector3 initialForward = initialRotation * Vector3.forward;
                Vector3 currentForward = currentRotation * Vector3.forward;
                Vector3 relativeForward = relativeRotation * Vector3.forward;

                Debug.DrawLine(Vector3.zero, initialForward, Color.blue);
                Debug.DrawLine(Vector3.zero, currentForward, Color.red);
                Debug.DrawLine(Vector3.zero, relativeForward, Color.green);
        

                Vector3 distanceVector = selectionManager.RayInitialDistance;
                Debug.DrawLine(selectionManager.RayInitialOrigin, distanceVector, Color.gray, 1f);

                //increment = UpdatePositionWithController(increment);                          
                // Vector3 rotatedVector = relativeRotation * (distanceVector*increment);
                Vector3 rotatedVector = relativeRotation * (2*distanceVector);
                
                Vector3 newPosition = rayComputation.RayOrigin + rotatedVector;


                Debug.DrawLine(rayComputation.RayOrigin, rotatedVector, Color.black, 1f);
                
                
                Vector3 movementVector = newPosition - transform.position;
                float movementDirection = Vector3.Dot(movementVector, beamDirection);
                float movementMagnitude = movementVector.magnitude;
                Vector3 originalPosition = transform.position;
                bool cond = !(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.1f) || (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.1f);
  // Calculate normalized time (progress), clamped between 0 and 1
                
                Vector3 targetPosition = originalPosition;
                float progress = 0;

                        // Smoothly interpolate the position using SmoothStep
                    
                if (!cond){
                            //
                    increment = 0;
                        if (OVRInput.Get(OVRInput.RawButton.RThumbstickLeft) || OVRInput.Get(OVRInput.RawButton.LThumbstickLeft)){
                            Debug.Log("Right Thumbstick Up pressed.");
                            //transform.Translate(Vector3.forward * movingStep);
                            increment = - 0.005f *beamLength;
                        }
                        if (OVRInput.Get(OVRInput.RawButton.RThumbstickRight) || OVRInput.Get(OVRInput.RawButton.LThumbstickRight)){
                            Debug.Log("Right Thumbstick Down pressed.");
                            //transform.Translate(Vector3.back * movingStep);
                            increment =  0.005f *beamLength;
                        }
                    targetPosition = originalPosition + beamDirection * increment;
                    elapsedTime1 += Time.deltaTime;
                    progress =  Mathf.Clamp01(elapsedTime1 / 1f);
                      

                        }
                        else{

                            increment = 0.05f *beamLength;
                            //targetPosition = originalPosition + Mathf.Sign(movementDirection) * movementMagnitude * Time.deltaTime * beamDirection;
                            if (movementMagnitude > 0.1*beamLength || movementMagnitude < -0.1*beamLength){
                                increment =  Mathf.Sign(movementDirection)*0.05f *beamLength * Time.deltaTime;
                            }
                            else{
                                increment = 0;
                            }
                            targetPosition = originalPosition +  increment  * beamDirection;
                            elapsedTime2 += Time.deltaTime;
                            progress = Mathf.Clamp01(elapsedTime2 / 1f);
                    

                }

                    float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

                    Vector3 expectedDistanceV1 = targetPosition - basePoint;
                    Vector3 expectedDistanceV2 = targetPosition - endPoint;
                    float maxDistance = (basePoint - endPoint).magnitude;
                    Debug.Log("LOADMOVER: Base point: " + basePoint + " End point: " + endPoint);
                    float expectedDistanceMagnitude = Math.Max(expectedDistanceV1.magnitude, expectedDistanceV2.magnitude);
                    Debug.Log("LOADMOVER: Expected distance: " + expectedDistanceMagnitude + " max distance: " + maxDistance);

                    if (expectedDistanceMagnitude <= maxDistance){
                        transform.position = Vector3.Lerp(originalPosition, targetPosition, smoothProgress);
                    }

                Debug.Log($"[ObjectMovement] Moving object to target position: {transform.position}");
                //transform.position = newPosition;
                // transform.position = newPosition + increment;
                

                //transform.rotation = relativeRotation;


            
        }
        else
        {
            // Reset the snapped state when deselected
            isSnapped = false;
        }
        }
    }

    private void SnapToRay()
    {   
        Vector3 distanceVector = selectionManager.RayInitialDistance;
        Vector3 rotatedVector = rayComputation.RayRotation * distanceVector;
        Vector3 targetPosition = rayComputation.RayOrigin + rotatedVector;
        // Align the object's position to the computed ray
        transform.position = targetPosition;
        //UPDATE IT TO
        //Debug.Log($"[ObjectMovement] Snapped object to initial position: {transform.position}, Ray Length: {rayComputation.RayLength}");

        // Mark snapping as complete
        isSnapped = true;
    }

    private void LogMovementInfo(Vector3 targetPosition)
    {
        float displacement = Vector3.Distance(transform.position, targetPosition);
        if (debugMovementQ){
            Debug.Log($"[ObjectMovement] Moving object to target position: {targetPosition}");
            Debug.Log($"[ObjectMovement] Current Object Position: {transform.position}");
            Debug.Log($"[ObjectMovement] Distance to Target: {displacement}");

            // Check if displacement changes dynamically
            Debug.Log($"[ObjectMovement] Displacement Dynamically Updated: {displacement > 0}");
        }
    }

    private float UpdatePositionWithController(float increment)
    {
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp)){
            Debug.Log("Right Thumbstick Up pressed.");
            //transform.Translate(Vector3.forward * movingStep);
            increment +=  movingStep;
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown)){
            Debug.Log("Right Thumbstick Down pressed.");
            //transform.Translate(Vector3.back * movingStep);
            increment += - movingStep;
        }
        return increment;
    }
}