using System.Collections.Generic;
using UnityEngine;

public class LoadGeometriesAuxiliaries : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is createdù
    private LoadingScheme loadingScheme;
    private List<Load> loads = new List<Load>(); // Editable in Inspector
    private List<Constraint> constraints = new List<Constraint>(); // Editable in Inspector
    private BeamPositioning beamPosition; 
    public LoadGeometries loadGeometries;

    public ConstraintsGeometries constraintsGeometries;
    public Material loadMaterial;
    public Material constrainMaterial;
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
            constraints = loadingScheme.GetConstraints();

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
                    ShowLoadObject(newObject);

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
                        ShowLoadObject(newCommonObject);
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

    public void UpdateConstraints()
    {
        // beamPosition = GetComponent<BeamPositioning>();
        if (beamPosition == null)
        {
            Debug.LogError("BeamPosition component not found on the GameObject!");
            return;
        }
        Vector3 basePoint = beamPosition.GetBaseCenter();
        Vector3 endPoint = beamPosition.GetFinalPoint();
        Vector3 beamNormal = beamPosition.GetNormal();
        float beamLength = beamPosition.GetBeamLength();

        foreach (Constraint constraint in constraints)
        {
            Vector3 position = basePoint + constraint.positionRatio * beamLength * beamNormal;
            //Vector3 position = new Vector3(0, 0, 0);

            
            if (constraint.GetObject() != null)
            {   
                float positionRatioOld = constraint.positionRatio;
                GameObject constraintObject = constraint.GetObject();
                Vector3 objectPosition = constraintObject.transform.position;
                float delta = (position - objectPosition).magnitude;
                if (delta > 0.01*beamLength){
                    float newRatio = (objectPosition - basePoint).magnitude / beamLength;
                    constraint.SetPositionRatio(newRatio);
                }
                
                constraint.SetPosition(objectPosition);   
                //constraintObject.transform.position = objectPosition;
                if (constraint.GetCommonObject() != null)
                {
                    GameObject commonJointObject = constraint.GetCommonObject();
                    commonJointObject.transform.position = objectPosition;
                }
            }
           
        }
    }


    public void SetConstraints()
    {
        // beamPosition = GetComponent<BeamPositioning>();
        if (beamPosition == null)
        {
            Debug.LogError("BeamPosition component not found on the GameObject!");
            return;
        }
        Vector3 basePoint = beamPosition.GetBaseCenter();
        Vector3 endPoint = beamPosition.GetFinalPoint();
        Vector3 beamNormal = beamPosition.GetNormal();
        float beamLength = beamPosition.GetBeamLength();
        Vector3 deflectionDirection = beamPosition.GetDeflectionDirection().normalized;

        foreach (Constraint constraint in constraints)
        {
            Vector3 position = basePoint + constraint.positionRatio * beamLength * beamNormal;
            //Vector3 position = new Vector3(0, 0, 0);

            constraint.SetPosition(position);
            
            if (constraint.GetObject() == null)
            {
                newObject = null;
                if (constraint.positionRatio == 0 || constraint.positionRatio == 1)
                {
                    constraint.internalQ = false;
                }
                else
                {
                    constraint.internalQ = true;
                }
                if (constraint.type == ConstraintType.Hinge)
                {
                    newObject = Instantiate(constraintsGeometries.hingeObject, constraint.GetPosition(), Quaternion.identity);
                    newCommonObject = Instantiate(constraintsGeometries.commonJointObject, constraint.GetPosition(), Quaternion.identity);

                }
                else if (constraint.type == ConstraintType.Roller)
                {
                    newObject = Instantiate(constraintsGeometries.rollerObject, constraint.GetPosition(), Quaternion.identity);
                    newCommonObject = Instantiate(constraintsGeometries.commonJointObject, constraint.GetPosition(), Quaternion.identity);
                }
                else
                {
                    Debug.LogError("Unknown constraint type: " + constraint.type);
                }
                
                // Debug.Log("Constraint name: " + constraint.name);
                // Debug.Log("Constraint position ratio: " + constraint.positionRatio);
                // Debug.Log("Constraint type: " + constraint.type);
                // Debug.Log("Constraint internal: " + constraint.internalQ);
                Debug.Log("Constraint position: " + constraint.GetPosition());
                Debug.Log("Constraint object: " + constraint.GetObject());

                Vector3 zAxis = Vector3.Cross(beamNormal, deflectionDirection).normalized;
                Vector3 yAxis = Vector3.Cross(zAxis, beamNormal).normalized;
                
                // Calculate rotation: Align z-axis (forward) with beamNormal
                Quaternion rotation = Quaternion.LookRotation(zAxis, yAxis);

                // Apply the rotation to the GameObject
                
                
                if (newObject != null)
                {   
                    
                        ConstraintMovement constraintMovementActual = newObject.GetComponent<ConstraintMovement>();
                        if (constraintMovementActual != null)
                        {
                            constraintMovementActual.SetMovable(constraint.movableQ);
                        }
                                    
                        newObject.transform.parent = null;
                        newObject.transform.SetParent(null); // Ensure it's not parented to anything
                        
                        newObject.transform.position = constraint.GetPosition(); // Set the position
                        float desiredHeight = 5*beamPosition.GetBeamSectionSize(); 
                        float originalZLength = GetObjectLocalLength(newObject); // Compute original Z length
                        
                        if (originalZLength > 0)
                        {
                            scaleFactor = 1.05f * desiredHeight; // Compute the scaling factor
                            Vector3 currentScale = newObject.transform.localScale; // Get the current scale
                            newObject.transform.localScale = new Vector3(currentScale.x * scaleFactor, currentScale.y * scaleFactor, currentScale.z * scaleFactor); // Apply the scaling
                        

                        }
                        else
                        {
                            Debug.LogError("Original Z-axis length is zero or could not be determined!");
                        }
                        
                        //newObject = SetRayInteractors(sourceObject, newObject);
                        
                        ConstraintMovement constraintMovement = newObject.GetComponent<ConstraintMovement>();
                        if (constraintMovement != null)
                        {
                            constraintMovement.SetBeamDirection(beamNormal);
                            constraintMovement.SetBeamLength(beamLength);
                            constraintMovement.SetBasePoint(basePoint);
                            constraintMovement.SetEndPoint(endPoint);
                        }
                        newObject.transform.rotation = rotation;
                        constraint.SetObject(newObject);
                        ShowConstraintObject(newObject);
                        
                    
                        if (newCommonObject != null){
                            newCommonObject.transform.parent = null;
                            newCommonObject.transform.SetParent(null); // Ensure it's not parented to anything
                            float actualHeight = GetObjectLocalLength(newCommonObject);
                            Debug.Log("Actual height: " + actualHeight);
                            float ratio = 0.35f;
                            float newScaleFactor = scaleFactor * ratio;
                            Vector3 currentScale = newCommonObject.transform.localScale; // Get the current scale
                            newCommonObject.transform.localScale = new Vector3(currentScale.x*newScaleFactor, currentScale.y*newScaleFactor, currentScale.z*newScaleFactor); // Apply the scaling
                            newCommonObject.transform.position = constraint.GetPosition();
                            
                            newCommonObject.transform.rotation = rotation;
                            constraint.SetCommonObject(newCommonObject);
                            ShowConstraintObject(newCommonObject);
                        }



                        newObject = null; // Reset the temporary object
                        newCommonObject = null;
                        
                    
                }
                else 
                {
                    Debug.LogError("Failed to instantiate the constraint object: " + constraint.name);
                }
            }
           
        }
    
    }


    private void ShowLoadObject(GameObject obj)
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


    private void ShowConstraintObject(GameObject obj)
    {
        // Enable the MeshRenderer to make the object visible
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
            renderer.material = constrainMaterial;
        }

        // Optionally, re-enable Colliders if needed
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

        private float GetObjectLocalLength(GameObject obj)
{
    MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
    if (meshFilter != null && meshFilter.mesh != null)
    {
        Bounds bounds = meshFilter.mesh.bounds;
        return bounds.size.z; // Local Z-length from the mesh bounds
    }
    else
    {
        Debug.LogError("No MeshFilter or mesh found on the object!");
        return 0f; // Default to zero if no mesh is found
    }
}


}

