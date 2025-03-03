using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public MeshGenerator beamGenerator;
    public PointerState pointerState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LoadingScheme loadingScheme;
    public float longPressDuration = 1.5f; // Time in seconds to consider a long press
    private float buttonPressTime = 0f;
    private bool isButtonHeld = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.Y)) {
            Debug.Log("Y pressed.");
            if (pointerState != null)
            {
                pointerState.DeactivatePointer();
                beamGenerator.SetBoolDrawToTrue();
                Debug.Log("Pointer deactivated.");
            }
         }

         if (OVRInput.GetDown(OVRInput.RawButton.X)) 
        {
            buttonPressTime = Time.time; // Store the time when button is first pressed
            isButtonHeld = true;
        }

        // If button is still held down
        if (OVRInput.Get(OVRInput.RawButton.X)) 
        {
            if (isButtonHeld && Time.time - buttonPressTime >= longPressDuration)
            {
                Debug.Log("X long pressed.");
                pointerState.ActivatePointer();
                beamGenerator.SetBoolDrawToFalse();
                MeshRenderer meshRenderer = beamGenerator.GetComponent<MeshRenderer>();
                meshRenderer.enabled = false;
                GameObject undeformedMeshObject = GameObject.Find("UndeformedMeshObject");
                Destroy(undeformedMeshObject);
                GameObject momentumMeshObject = GameObject.Find("MomentumMeshObject");
                Destroy(momentumMeshObject);
                GameObject momentumFillMeshObject = GameObject.Find("MomentumFillMeshObject");
                Destroy(momentumFillMeshObject);
                List<Load> loads = loadingScheme.GetLoads();
                foreach (Load load in loads)
                {
                    GameObject magnitudeObject = load.GetMagnitudeObject();
                    Destroy(magnitudeObject);
                    GameObject pointerObject = load.GetPointerObject();
                    Destroy(pointerObject);
                }
                List<Constraint> constraints = loadingScheme.GetConstraints();
                foreach (Constraint constraint in constraints)
                {
                    GameObject constraintObject = constraint.GetObject();
                    Destroy(constraintObject);
                    GameObject constraintCommonObject = constraint.GetCommonObject();
                    Destroy(constraintCommonObject);
                }
                
                isButtonHeld = false; // Reset to prevent multiple triggers
            }
        }

        // Reset if the button is released
        if (OVRInput.GetUp(OVRInput.RawButton.X)) 
        {
            isButtonHeld = false;
        }
    }
}
