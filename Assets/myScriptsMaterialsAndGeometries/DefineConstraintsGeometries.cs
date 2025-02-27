using UnityEngine;

public class ConstraintsGeometries : MonoBehaviour
{

    public GameObject hingeObject; // Assign a 3D object prefab in the Inspector
    public GameObject rollerObject; // Assign a 3D object prefab in the Inspector

    public GameObject commonJointObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material constraintsMaterial;

    public Material getConstraintsMaterial()
    {
        return constraintsMaterial;
    }
    void Start()
    {
        // Hide the hingeObject and rollerObject visually but keep them functional
        if (hingeObject != null)
        {
            //AdjustPivot(hingeObject);
            NormalizeObject(hingeObject);
            hingeObject.transform.position = new Vector3(0, 0, 0);

            // Canvas canvas = hingeObject.GetComponent<Canvas>();
            // if (canvas != null)
            // {
            //     canvas.renderMode = RenderMode.WorldSpace;
            // }
            // hingeObject.transform.position = new Vector3(0, 0, 0);
            // // get body before continuing
            // Debug.Log("Bounds: " + hingeObject.GetComponent<MeshFilter>().mesh.bounds);
            
            HideObject(hingeObject);
            
        }
        if (rollerObject != null)
        {
            //AdjustPivot(hingeObject);
            NormalizeObject(rollerObject);
            rollerObject.transform.position = new Vector3(0, 0, 0);
            HideObject(rollerObject);
        }
        if (commonJointObject != null)
        {
            
            NormalizeObject(commonJointObject, true);
            commonJointObject.transform.position = new Vector3(0, 0, 0);
            HideObject(commonJointObject);
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


    public void NormalizeObject(GameObject obj, bool offsetQ = false)
    {
        // Find the MeshFilter in the object or its children
        MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            // Get the mesh bounds in local space
            Bounds bounds = meshFilter.mesh.bounds;
            Debug.Log("Original bounds: " + bounds);
            
            Debug.Log("Original position: " + meshFilter.transform.position);
            Debug.Log("Original scale: " + meshFilter.transform.localScale);
            
            // Find the largest dimension
            //float largestDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            float largestDimension = bounds.size.z;

            if (largestDimension > 0)
            {
                // Compute the scale factor to normalize the largest dimension to 1
                float scaleFactor = 1.0f / largestDimension;

                // Rescale the object
                meshFilter.transform.localScale *= scaleFactor;

                Quaternion targetRotation = Quaternion.Euler(-90, 0, 0); // Rotate 90° around X-axis
                meshFilter.transform.rotation = targetRotation;
                // Center the object
                Vector3 offset = bounds.center;
                //meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) + new Vector3(0, 0, meshFilter.transform.position.z);
                if (offsetQ)
                {
                    meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position;// + new Vector3(0, meshFilter.transform.position.y/2, 0);
                }
                else
                {
                    meshFilter.transform.position -= meshFilter.transform.TransformPoint(offset) - meshFilter.transform.position - new Vector3(0, meshFilter.transform.position.y, 0);
                }
                
                //obj.transform.position -= obj.transform.TransformPoint(offset) - obj.transform.position;
                

                Debug.Log($"Normalized {obj.name}: Scale Factor = {scaleFactor}, Offset = {offset}");


    // Find the Renderer for the object
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                if (renderer != null && constraintsMaterial != null)
                {
                    // Assign the material
                    renderer.material = constraintsMaterial;
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
