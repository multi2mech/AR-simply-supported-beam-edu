using System.Numerics;
using UnityEngine;

public class BeamPositioning : MonoBehaviour
{
    [Header("Start and end markers")]
    // public Vector3 centerOffset = Vector3.zero; // Offset for the center point
    
    public GameObject start; // Start point object
    public GameObject end; // End point object

    [Header("Beam Properties")]
    //public float beamLength = 5f; // Length of the beam (scalar)
    //public Vector3 beamDirection = Vector3.forward; // Direction of the beam
    private UnityEngine.Vector3 beamDirection;
    private float beamLength;
    private UnityEngine.Vector3 beamVector;
    private UnityEngine.Vector3 basePoint;
    private UnityEngine.Vector3 endPoint;
    private float beamSectionSize;

    void Start()
    {
        UpdateBeamProperties();
    }
   void Update()
    {
        // Optionally update the final point offset dynamically during runtime
        UpdateBeamProperties();

    }

    public UnityEngine.Vector3 GetDeflectionDirection()
    {
        // Define a "downward" direction
        UnityEngine.Vector3 downwardDirection = UnityEngine.Vector3.back;

        // Ensure the "bottom" vector is perpendicular to the beamDirection
        UnityEngine.Vector3 bottomVector = UnityEngine.Vector3.Cross(beamDirection, downwardDirection).normalized;

        return bottomVector;
    }
    private void UpdateBeamProperties()
    {
        // Compute the beam direction based on the start and end points
        basePoint = start.transform.position;
        endPoint = end.transform.position;
        beamVector = endPoint - basePoint;
        beamDirection = beamVector.normalized;
        beamLength = beamVector.magnitude;
        
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