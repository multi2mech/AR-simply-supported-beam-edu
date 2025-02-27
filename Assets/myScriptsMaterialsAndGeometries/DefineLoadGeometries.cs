using System;
using UnityEngine;

public class LoadGeometries : MonoBehaviour
{

    public GameObject forceObject; // Assign a 3D object prefab in the Inspector
    public GameObject torqueObject; // Assign a 3D object prefab in the Inspector

    public GameObject commonPointerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material loadsMaterial;
    float largestDimension = 0.0f;
    float largestDimensionScaled = 0.0f;
    float pointerDimension = 0.0f;
    float pointerDimensionScaled = 0.0f;

    public float GetPointerDimensionScaled()
    {
        return pointerDimensionScaled;
    }

    public Material getloadsMaterial()
    {
        return loadsMaterial;
    }


    void UpdateLargestDimension()
    {   
        if (forceObject != null){
        largestDimension = GetLargestDimension(forceObject);
        }
        //set both such tath (force + point) = 1
        if (commonPointerObject != null){
            largestDimension += GetLargestDimension(commonPointerObject);
        }

    }

    void UpdatePointerDimension()
    {   
        if (commonPointerObject != null){
        // Find the MeshFilter in the object or its children
            pointerDimension = GetLargestDimension(commonPointerObject);
        }
    }

    void SetLargestDimensionScaled(float dimension)
    {
        largestDimensionScaled = dimension;
    }

    void SetPointerDimensionScaled(float dimension, GameObject obj)
    {
        pointerDimensionScaled = dimension;
        ScalingProperties scalingProperties = obj.GetComponent<ScalingProperties>();
        if (scalingProperties != null)
        {
            scalingProperties.SetPointerSizeScaled(dimension);
        }
    }

    public float GetLargestDimension(GameObject obj)
    {
        float size = 0.0f;
        if (obj != null){
        MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            // Get the mesh bounds in local space
            Bounds bounds = meshFilter.mesh.bounds;
            // Debug.Log("Original bounds: " + bounds);
            // Debug.Log("Original position: " + meshFilter.transform.position);
            // Debug.Log("Original scale: " + meshFilter.transform.localScale);

            size = bounds.size.z;
        }
        }

        return size;

    }

    void Start()
    {
        UpdateLargestDimension();
        UpdatePointerDimension();
        // Hide the forceObject and torqueObject visually but keep them functional
        if (forceObject != null)
        {
            NormalizeObject(forceObject, 'f');
            ResetObjectPosition(forceObject, 'f');
            forceObject.transform.position = new Vector3(0, 0, 0);

            HideObject(forceObject);
            
        }
        if (torqueObject != null)
        {
            //AdjustPivot(forceObject);
            NormalizeObject(torqueObject, 't');
            ResetObjectPosition(torqueObject, 't');
            torqueObject.transform.position = new Vector3(0, 0, 0);
            HideObject(torqueObject);
        }
        if (commonPointerObject != null)
        {
            
            NormalizeObject(commonPointerObject, 'c');
            ResetObjectPosition(commonPointerObject, 'c');
            commonPointerObject.transform.position = new Vector3(0, 0, 0);
            HideObject(commonPointerObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideObject(GameObject obj)
    {
        // Disable the MeshRenderer to make the object invisible
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }

        // Optionally, you can disable Colliders as well if you don't want them to interact physically
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }


    public void NormalizeObject(GameObject obj, char? flag = null)
    {
        // Find the MeshFilter in the object or its children
        MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            Bounds bounds = meshFilter.mesh.bounds;// Get the mesh bounds in local space

            if (largestDimension > 0)
            {
                // Compute the scale factor to normalize the largest dimension to 1
                float scaleFactor = 1.0f / largestDimension;
                // Rescale the object
                meshFilter.transform.localScale *= scaleFactor;
                Quaternion targetRotation = Quaternion.Euler(-90, 0, 0); // Rotate 90° around X-axis
                meshFilter.transform.rotation = targetRotation;
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                
                SetPointerDimensionScaled(pointerDimension * scaleFactor, obj);
            
                if (renderer != null && loadsMaterial != null)
                {
                    // Assign the material
                    renderer.material = loadsMaterial;
                    Debug.Log($"Assigned custom material to {obj.name}");
                }
                else
                {
                    Debug.LogWarning($"Renderer or custom material not found for {obj.name}");
                }

            }
            else
            {
                Debug.LogError($"Bounds size is zero for {obj.name}. Cannot normalize.");
            }
        }
        else
        {
            Debug.LogError($"No MeshFilter found in {obj.name} or its children.");
        }
    }



void ResetObjectPosition(GameObject obj, char? flag = null){
    MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
    if (meshFilter != null)
    {
        Bounds bounds = meshFilter.mesh.bounds;
        Vector3 offset = bounds.center;
        LoadMovement moveLoad = obj.GetComponent<LoadMovement>();
        if (flag == null)
        {
            meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position;// + new Vector3(0, meshFilter.transform.position.y/2, 0);
        }
        else if (flag == 'f')
        {
            //Debug.Log("Force object");
            //meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position - new Vector3(0, meshFilter.transform.position.y, 0);
            meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position;
            meshFilter.transform.position -= new Vector3(0, meshFilter.transform.position.y/2 - pointerDimension/largestDimension, 0 );
            // Debug.Log("Force object z:" + meshFilter.transform.position.z);
            if (moveLoad != null)
            {
                moveLoad.SetMovableQ(false); // only the common pointer can be moved by the user
                moveLoad.SetScalableQ(true);
            }
        }
        else if (flag == 't')
        {
            meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position - new Vector3(0, meshFilter.transform.position.y, 0);
        }
        else if (flag == 'c')
        {
            meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position;
            meshFilter.transform.position += new Vector3(0, meshFilter.transform.position.y + (1.1f)*pointerDimension/largestDimension, 0 );
            if (moveLoad != null)
            {
                moveLoad.SetScalableQ(false); // only the common pointer can be moved by the user
            }
        }
        else
        {
            meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position;
        }
    }
}

    void AdjustPivot(GameObject obj)
{
    // Find the MeshFilter to get the bounds
    MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
    if (meshFilter != null)
    {
        // Get the bounds of the mesh in local space
        Bounds bounds = meshFilter.mesh.bounds;

        // Calculate the desired pivot offset
        Vector3 localOffset = bounds.center; // Center on X and Y
        localOffset.z = bounds.max.z; // Top on Z

        // Adjust the position of the object to compensate for the pivot offset
        obj.transform.position += obj.transform.TransformVector(localOffset);

        // Adjust the mesh's vertices to shift the geometry relative to the new pivot
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= localOffset; // Shift each vertex by the local offset
        }

        // Update the mesh with the adjusted vertices
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        Debug.Log($"Adjusted Pivot for {obj.name}: Offset = {localOffset}");
    }
    else
    {
        Debug.LogError($"No MeshFilter found on {obj.name}");
    }
}




}
