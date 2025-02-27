using System.Collections.Generic;
using UnityEngine;

public class LoadGeometriesAuxiliaries : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is createdù
    private LoadingScheme loadingScheme;
    private List<Load> loads = new List<Load>(); // Editable in Inspector
    private BeamPositioning beamPosition; 
    public LoadGeometries loadGeometries;
    public Material loadMaterial;
    private GameObject newObject;
    private GameObject newCommonObject;
    private float loadOriginalZLength = 1f;
    private float scaleFactor = 0;
    void Start()
    {
        loadingScheme = GetComponent<LoadingScheme>();
        if (loadingScheme == null)
        {
            Debug.LogError("LoadingScheme component not found on the GameObject!");
            return;
        }

        Transform parent = transform.parent; // Get the parent of A

        if (parent != null)
        {
            beamPosition = FindBeamPositioning(parent);
            
            if (beamPosition != null)
            {
                Debug.Log("BeamPositioning found!");
            }
            else
            {
                Debug.LogWarning("BeamPositioning not found in any children.");
            }
        }
        
    }

    BeamPositioning FindBeamPositioning(Transform parent)
{
    foreach (Transform child in parent)
    {
        BeamPositioning beamPosition = child.GetComponent<BeamPositioning>();
        
        if (beamPosition != null)
        {
            return beamPosition; // Found the component
        }

        // Recursively search in deeper children
        beamPosition = FindBeamPositioning(child);
        if (beamPosition != null)
        {
            return beamPosition; // Found in deeper levels
        }
    }

    return null; // Not found
}

    // Update is called once per frame
    void Update()
    {
        if (loadingScheme != null)
        {
            loads = loadingScheme.GetLoads();

        }
        
    }


    public void UpdateLoads()
    {
        //beamPosition = GetComponent<BeamPositioning>();
        if (beamPosition == null)
        {
            Debug.LogError("BeamPosition component not found on the GameObject!");
            return;
        }
        Vector3 basePoint = beamPosition.GetBaseCenter();
        Vector3 endPoint = beamPosition.GetFinalPoint();
        Vector3 beamNormal = beamPosition.GetNormal();
        float beamLength = beamPosition.GetBeamLength();

        foreach (Load load in loads)
        {
            Vector3 position = basePoint + load.positionRatio * beamLength * beamNormal;
            //Vector3 position = new Vector3(0, 0, 0);

            
            if (load.GetPointerObject() != null)
            {   
                // for loads the position driver is the pointer:

                float positionRatioOld = load.positionRatio;
                GameObject pointerObejct = load.GetPointerObject();
                Vector3 objectPosition = pointerObejct.transform.position;
                float delta = (position - objectPosition).magnitude;
                if (delta > 0.01*beamLength){
                    float newRatio = (objectPosition - basePoint).magnitude / beamLength;
                    load.SetPositionRatio(newRatio);
                }
                
                load.SetPosition(objectPosition);   
                //constraintObject.transform.position = objectPosition;
                if (load.GetMagnitudeObject() != null)
                {
                    GameObject magnitudeObject = load.GetMagnitudeObject();
                    magnitudeObject.transform.position = objectPosition;
                }
            }
           
        }
    }

    public void SetLoads()
    {

        if (beamPosition == null)
        {
            Debug.LogError("BeamPosition component not found on the GameObject!");
            return;
        }
        Vector3 basePoint = beamPosition.GetBaseCenter();
        
        // Debug.Log("Base point: " + basePoint);
        Vector3 beamNormal = beamPosition.GetNormal();
        float beamLength = beamPosition.GetBeamLength();
        Vector3 endPoint = basePoint + beamLength * beamNormal;
        // Debug.Log("End point: " + endPoint);
        Vector3 deflectionDirection = beamPosition.GetDeflectionDirection();

        foreach (Load load in loads)
        {
            Vector3 position = basePoint + load.positionRatio * beamLength * beamNormal;
            //Vector3 position = new Vector3(0, 0, 0);

            load.SetPosition(position);
            if (load.positionRatio == 0 || load.positionRatio == 1)
                {
                    load.internalQ = false;
                }
                else
                {
                    load.internalQ = true;
                }
            if (load.GetMagnitudeObject() == null)
            {
                newObject = null;
            
                if (load.type == LoadType.Force)
                {
                    newObject = Instantiate(loadGeometries.forceObject, load.GetPosition(), Quaternion.identity);
                    newCommonObject = Instantiate(loadGeometries.commonPointerObject, load.GetPosition(), Quaternion.identity);
                    loadOriginalZLength = loadGeometries.GetLargestDimension(newObject);

                }
                else if (load.type == LoadType.Torque)
                {
                    newObject = Instantiate(loadGeometries.torqueObject, load.GetPosition(), Quaternion.identity);
                    newCommonObject = Instantiate(loadGeometries.commonPointerObject, load.GetPosition(), Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Unknown load type: " + load.type);
                }
                

                // Debug.Log("Loads position: " + load.GetPosition());
                // Debug.Log("Load object: " + load.GetMagnitudeObject());
                
                if (newObject!= null){
                    LoadMovement mover = newObject.GetComponent<LoadMovement>();
                    mover.SetMovableQ(false);
                    mover.SetScalableQ(load.scalableQ);
                }

                if (newCommonObject!= null){
                    LoadMovement mover = newCommonObject.GetComponent<LoadMovement>();
                    mover.SetMovableQ(load.movableQ);
                    mover.SetScalableQ(false);
                }
                Vector3 zAxis = Vector3.Cross(beamNormal, deflectionDirection).normalized;
                Vector3 yAxis = Vector3.Cross(zAxis, beamNormal).normalized;
                Debug.Log("Beam normal: " + beamNormal);
                Debug.Log("Deflection direction: " + deflectionDirection);
                // Calculate rotation: Align z-axis (forward) with beamNormal
                Quaternion rotation = Quaternion.LookRotation(zAxis, yAxis);
                Debug.Log("Rotation: " + rotation);
                Debug.Log("Axis z: " + zAxis);
                Debug.Log("Axis y: " + yAxis);
                if (newObject != null)
                {   
                    newObject.transform.parent = null;
                    newObject.transform.SetParent(null); // Ensure it's not parented to anything  
                    newObject.transform.position = load.GetPosition(); // Set the position
                    float desiredHeight = 10*beamPosition.GetBeamSectionSize(); // Get the desired scale
                    Debug.Log("Desired height: " + desiredHeight);
                    if (loadOriginalZLength > 0)
                    {
                        scaleFactor = desiredHeight; // This is prescaled with respect to the body height which is 1
                        Vector3 currentScale = newObject.transform.localScale; // Get the current scale
                        newObject.transform.localScale = new Vector3(currentScale.x * scaleFactor, currentScale.y * scaleFactor, currentScale.z * scaleFactor); // Apply the scaling
                        Debug.Log("Current scale: " + newObject.transform.localScale);
                        Debug.Log("Original Z-axis length: " + loadOriginalZLength);
                        Debug.Log("Scale factor: " + scaleFactor);
                    }
                    else
                    {
                        Debug.LogError("Original Z-axis length is zero or could not be determined!");
                    }
                    newObject.transform.rotation = rotation;
                    load.SetMagnitudeObject(newObject);
                    ShowObject(newObject);

                    LoadMovement loadMovemenet = newObject.GetComponent<LoadMovement>();
                    if (loadMovemenet != null)
                    {
                        loadMovemenet.SetOriginalHeight(load.GetReferenceHeight());
                        loadMovemenet.SetScales(load.minMultiplier, load.maxMultiplier);
                        //Debug.Log("Setting base point: " + basePoint);
                        
                        //Debug.Log("Setting end point: " + endPoint);
                    }

                    loadMovemenet = newCommonObject.GetComponent<LoadMovement>();
                    if (loadMovemenet != null)
                    {
                        loadMovemenet.SetBasePoint(basePoint);
                        loadMovemenet.SetEndPoint(endPoint);
                    }
                    
                    
                
                    if (newCommonObject != null){
                        newCommonObject.transform.parent = null;
                        newCommonObject.transform.SetParent(null); // Ensure it's not parented to anything
                        loadMovemenet = newCommonObject.GetComponent<LoadMovement>();
                        if (loadMovemenet != null)
                        {
                            loadMovemenet.SetBeamDirection(beamNormal);
                            loadMovemenet.SetBeamLength(beamLength);
                        }

                        float newScaleFactor = scaleFactor * 1f; // this is prescaled with respect to the body height
                        Vector3 currentScale = newCommonObject.transform.localScale; // Get the current scale
                        newCommonObject.transform.localScale = new Vector3(currentScale.x*newScaleFactor, currentScale.y*newScaleFactor, currentScale.z*newScaleFactor); // Apply the scaling
                        newCommonObject.transform.position = load.GetPosition();
                        newCommonObject.transform.rotation = rotation;
                        load.SetPointerObject(newCommonObject);
                        ShowObject(newCommonObject);
                    }



                    newObject = null; // Reset the temporary object
                    newCommonObject = null;
                        
                    
                }
                else 
                {
                    Debug.LogError("Failed to instantiate the load object: " + load.name);
                }
            }
           
        }
    }


        private void ShowObject(GameObject obj)
    {
        // Enable the MeshRenderer to make the object visible
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
            renderer.material = loadMaterial;
        }

        // Optionally, re-enable Colliders if needed
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }


}

