using System.Numerics;
using UnityEngine;

public class BeamPositioning : MonoBehaviour
{
    [Header("Start and end markers")]
    // public Vector3 centerOffset = Vector3.zero; // Offset for the center point
    
    public GameObject start; // Start point object
    public GameObject end; // End point object

    public GameObject reference; // Reference point object
    private UnityEngine.Vector3 referencePoint;

    [Header("Beam Properties")]
    //public float beamLength = 5f; // Length of the beam (scalar)
    //public Vector3 beamDirection = Vector3.forward; // Direction of the beam
    private UnityEngine.Vector3 beamDirection;
    private float beamLength;
    private UnityEngine.Vector3 beamVector;
    private UnityEngine.Vector3 basePoint;
    private UnityEngine.Vector3 endPoint;
    private float beamSectionSize;

    private UnityEngine.Vector3 deflectionVector;
    private UnityEngine.Vector3 cameraForward;
    void Start()
    {   
        referencePoint = reference.transform.position;
        // Define "up" based on the camera (or world)
        cameraForward = Camera.main.transform.forward; // or HMD forward
        UpdateBeamProperties();
    }

    public void UpdateRefencePoint()
    {
        referencePoint = reference.transform.position;
    }

   void Update()
    {
        // Optionally update the final point offset dynamically during runtime
        //Debug.Log("Reference point: " + referencePoint);
       // UpdateBeamProperties();

    }

    public UnityEngine.Vector3 GetDeflectionDirection()
    {
        return deflectionVector;

    }

    public void ComputeDeflectionDirection()
    {
        
        // Define a "downward" direction
        //UnityEngine.Vector3 downwardDirection = UnityEngine.Vector3.back;

        // Ensure the "bottom" vector is perpendicular to the beamDirection
        //UnityEngine.Vector3 bottomVector = UnityEngine.Vector3.Cross(beamDirection, downwardDirection).normalized;

        // // return bottomVector;
        UnityEngine.Vector3 globalDown =  UnityEngine.Vector3.down;

        // // Step 1: Project global down onto the plane perpendicular to beamDirection
        UnityEngine.Vector3 bottomVector = UnityEngine.Vector3.ProjectOnPlane(globalDown, beamDirection).normalized;
        
        // If it ends up pointing upward, flip it
        if (UnityEngine.Vector3.Dot(bottomVector, UnityEngine.Vector3.down) > 0)
        {
            bottomVector = -bottomVector;
        }
        
        deflectionVector = bottomVector;
    }
    public void UpdateBeamProperties()
    {
        // Compute the beam direction based on the start and end points
        basePoint = start.transform.position;
        endPoint = end.transform.position;
        
        // check triangle ABC orientation
        UnityEngine.Vector3 AB = endPoint - basePoint;
        UnityEngine.Vector3 AC = referencePoint - basePoint;

        // Cross product gives the triangle's normal direction
        UnityEngine.Vector3 normal = UnityEngine.Vector3.Cross(AB, AC);
        

        // Dot with camera's forward direction
        float orientation = UnityEngine.Vector3.Dot(normal.normalized, cameraForward.normalized);
        
        if (orientation > 0)
        {
            // Swap the start and end points
            UnityEngine.Vector3 temp = basePoint;
            basePoint = endPoint;
            endPoint = temp;
        }

        beamVector = endPoint - basePoint;
        beamDirection = beamVector.normalized;
        beamLength = beamVector.magnitude;
        ComputeDeflectionDirection();
    }

    public float GetBeamSectionSize()
    {
        return beamSectionSize;
    }

    public void SetBeamSectionSize(float size)
    {
        beamSectionSize = size;
    }
    public UnityEngine.Vector3 GetBaseCenter()
    {
        return basePoint;
    }

    public UnityEngine.Vector3 GetFinalPoint()
    {
        return endPoint;
    }

    public UnityEngine.Vector3 GetNormal()
    {

        return beamDirection.normalized;
    }

    public float GetBeamLength()
    {
        return beamLength;
    }

    public UnityEngine.Vector3 GetBeamVector()
    {
        return beamVector;
    }

}