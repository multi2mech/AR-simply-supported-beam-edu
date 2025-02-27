using UnityEngine;

public class updatedMaterialsForPointers : MonoBehaviour
{
    public Material pointerRest;  // Material for the "rest" state
    public Material pointerActive; // Material for the "active" state
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Renderer objectRenderer;
    void Start()
    {
        // Get the Renderer component of the object
        objectRenderer = GetComponent<Renderer>();

        // Ensure the object has a Renderer
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this object.");
            return;
        }
    }

    // Method to set both materials on the object
    public void SetDoubleMaterial()
    {
        if (pointerRest != null && pointerActive != null)
        {
            // Assign the materials to the Renderer
            objectRenderer.materials = new Material[] { pointerRest, pointerActive };
        }
        else
        {
            Debug.LogError("One or both materials are not assigned.");
        }
    }

      public void SetSingleMaterial()
    {
        if (pointerRest != null && pointerActive != null)
        {
            // Assign the materials to the Renderer
            objectRenderer.materials = new Material[] { pointerRest };
        }
        else
        {
            Debug.LogError("One or both materials are not assigned.");
        }
    }
}
