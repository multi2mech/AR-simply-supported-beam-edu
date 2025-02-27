using UnityEngine;

public class updatedMaterialsForConstraint : MonoBehaviour
{
    public Material pointerRest;  // Material for the "rest" state
    public Material pointerActive; // Material for the "active" state
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Renderer objectRenderer;
    private ConstraintMovement constraintMovement;
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
        bool movableQ = false;
        constraintMovement = GetComponentInParent<ConstraintMovement>();
        if (constraintMovement == null)
        {
            Debug.LogError("ConstraintMovement not found on " + gameObject.name);
        }
        else{
            movableQ = constraintMovement.isMovableQ();
        }
        
        if (pointerRest != null && pointerActive != null && movableQ)
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
